using System.Collections.Generic;
using Ecs.Item;
using UnityEngine.Assertions;
using Utopia;

namespace Ecs.Inventory {
	[InstallerGenerator(InstallerId.Game)]
	public sealed class CellManipulator : ICellManipulator {
		private static Dictionary<ItemId, int> _pack = new();
		private static PackedItemsComparer _comparer = new();
		private static readonly List<InventoryEntity> Transaction = new(8);
		private readonly IItemsDatabase _itemsDatabase;
		private readonly IItemsFactory _itemsFactory;
		private readonly ICellHelper _cellHelper;

		public CellManipulator(
			IItemsDatabase itemsDatabase,
			IItemsFactory itemsFactory,
			ICellHelper cellHelper
		) {
			_itemsDatabase = itemsDatabase;
			_itemsFactory = itemsFactory;
			_cellHelper = cellHelper;
		}

		public bool Put(InventoryEntity cell, ItemId itemId, int quantity) {
			Assert.IsTrue(quantity > 0, $"Amount less than 1 ({quantity})!");
			var commit = false;

			var item = _itemsDatabase.Get(itemId);
			var referencedQuantity = quantity;

			var result = TryPutInUsedCell(item, cell, ref referencedQuantity);
			if (result) {
				commit = Commit(item, quantity);
				Transaction.Clear();
				return commit;
			}

			var quantityInEmptyCell = referencedQuantity;
			result = TryPutInEmptyCell(item, cell, ref referencedQuantity);
			if (result)
				commit = Commit(item, quantity - quantityInEmptyCell);
			Transaction.Clear();
			return result && commit;
		}

		public (bool allPut, int remainder) PutAny(InventoryEntity cell, ItemId itemId, int quantity) {
			Assert.IsTrue(quantity > 0, "Amount less than 1!");

			var item = _itemsDatabase.Get(itemId);
			var referencedQuantity = quantity;
			var result = TryPutInUsedCell(item, cell, ref referencedQuantity);

			Commit(item, quantity);
			Transaction.Clear();
			if (result)
				return (true, 0);

			var quantityInEmptyCell = referencedQuantity;
			result = TryPutInEmptyCell(item, cell, ref referencedQuantity);
			return (result, quantityInEmptyCell);
		}

		private bool Commit(Item.ItemData item, int quantity) {
			foreach (var cell in Transaction) {
				var cellQuantity = _cellHelper.Quantity(cell);
				if (cellQuantity + quantity <= item.StackSize) {
					_cellHelper.Add(cell, quantity);
					return true;
				}

				var count = item.StackSize - cellQuantity;
				_cellHelper.Add(cell, count);
				quantity -= count;
			}

			return true;
		}

		private bool TryPutInUsedCell(Item.ItemData item, InventoryEntity cell, ref int quantity) {
			if (!item.IsStackable)
				return false;

			if (CellCantAcceptThatItem(cell, item))
				return false;

			var cellQuantity = _cellHelper.Quantity(cell);
			if (cellQuantity + quantity <= item.StackSize) {
				Transaction.Add(cell);
				return true;
			}

			var count = item.StackSize - cellQuantity;
			Transaction.Add(cell);
			quantity -= count;

			return false;
		}

		private bool TryPutInEmptyCell(Item.ItemData item, InventoryEntity cell, ref int quantity) {
			if (!cell.IsEmpty || cell.IsBroken)
				return false;

			var instance = _itemsFactory.Create(item.Id);
			if (quantity > item.StackSize) {
				quantity -= item.StackSize;
				_cellHelper.SetItem(cell, instance, item.StackSize);
				return false;
			}

			_cellHelper.SetItem(cell, instance, quantity);
			return true;
		}

		public void Remove(InventoryEntity cell, ItemId itemId, ref int amount) {
			if (!_cellHelper.Contains(cell, itemId))
				return;

			var quantity = _cellHelper.Quantity(cell);
			if (quantity >= amount) {
				_cellHelper.Remove(cell, amount);
				return;
			}

			_itemsFactory.Destroy(cell.CellTarget.Value);
			_cellHelper.Reset(cell);
			amount -= quantity;
		}

		public void RemoveAll(InventoryEntity cell, ItemId itemId) {
			if (!_cellHelper.Contains(cell, itemId))
				return;
			_itemsFactory.Destroy(cell.CellTarget.Value);
			_cellHelper.Reset(cell);
		}

		public bool IsEnoughSpace(InventoryEntity cells, ItemId itemId, int amount) {
			Assert.IsTrue(amount > 0, "Amount less than 1!");

			var quantity = amount;

			var item = _itemsDatabase.Get(itemId);

			return CanFitInUsedCell(item, cells, ref quantity) ||
			       CanFitInEmptyCell(item, cells, ref quantity) ||
			       CanMerge(quantity, amount);
		}

		public bool IsEnoughSpace(InventoryEntity entity, IReadOnlyList<(ItemId itemId, int amount)> items) {
			var emptyCells = 0;
			emptyCells += entity.IsEmpty && !entity.IsBroken ? 1 : 0;

			if (emptyCells >= items.Count)
				return true;

			foreach (var (itemId, amount) in items) {
				Assert.IsTrue(amount > 0, "Amount less than 1!");

				var item = _itemsDatabase.Get(itemId);
				var quantity = amount;
				// Если будут предметы одного типа, это может вызвать ошибку!
				// Т.е. может быть такое, что два предмета в списке попадут в одну ячейку с предметом
				if (CanFitInUsedCell(item, entity, ref quantity) || CanMerge(quantity, amount))
					continue;

				if (!CanFitInEmptyCell(item, entity, ref quantity))
					return false;

				emptyCells--;
				if (emptyCells >= 0)
					continue;
				return false;
			}

			return emptyCells >= 0;
		}

		/// <summary>
		/// Если это патроны для оружия например, и мы можем поднять хотя бы один патрон из пачки
		/// </summary>
		private static bool CanMerge(int quantity, int amount) => amount - quantity > 0;

		private bool CanFitInUsedCell(Item.ItemData item, InventoryEntity cell, ref int quantity) {
			if (!item.IsStackable)
				return false;

			var stackSize = item.StackSize;

			if (CellCantAcceptThatItem(cell, item))
				return false;

			var cellQuantity = _cellHelper.Quantity(cell);
			if (cellQuantity + quantity <= stackSize)
				return true;

			quantity -= stackSize - cellQuantity;

			return false;
		}

		private bool CanFitInEmptyCell(Item.ItemData item, InventoryEntity entity, ref int quantity) {
			if (entity.IsEmpty && !entity.IsBroken) {
				if (quantity > item.StackSize)
					quantity -= item.StackSize;
				else
					return true;
			}

			return false;
		}

		private bool CellCantAcceptThatItem(InventoryEntity cell, Item.ItemData item)
			=> _cellHelper.Quantity(cell) >= item.StackSize || !_cellHelper.Contains(cell, item.Id);

		public int Count(InventoryEntity cell, ItemId itemId) {
			var sum = 0;
			if (_cellHelper.Contains(cell, itemId))
				sum += _cellHelper.Quantity(cell);

			return sum;
		}

		public bool Contains(InventoryEntity cell, ItemId itemId) => _cellHelper.Contains(cell, itemId);

		public (bool, ItemId) ContainsAnyOf(InventoryEntity cell, IEnumerable<ItemId> items) {
			foreach (var item in items)
				if (_cellHelper.Contains(cell, item))
					return (true, item);
			return (false, default);
		}

		public (bool, ItemInstanceId) FindAnyInstanceOf(InventoryEntity cell, ItemId item)
			=> _cellHelper.Contains(cell, item) ? (true, cell.CellTarget.Value) : (false, default);

		public (bool, ItemInstanceId) FindAnyInstanceOf(InventoryEntity cell, IEnumerable<ItemId> items) {
			foreach (var item in items)
				if (_cellHelper.Contains(cell, item))
					return (true, cell.CellTarget.Value);
			return (false, default);
		}

		public bool FindAllInstancesOf(InventoryEntity cell, IEnumerable<ItemId> items, List<ItemInstanceId> buffer) {
			buffer.Clear();
			foreach (var item in items)
				if (_cellHelper.Contains(cell, item))
					buffer.Add(cell.CellTarget.Value);
			return buffer.Count > 0;
		}

		public void PackedItems(InventoryEntity cell, List<(ItemId, int)> buffer, bool sortSmallToLarge = true) {
			if (cell.IsEmpty)
				return;

			var itemId = _cellHelper.GetItemId(cell);
			if (!_pack.ContainsKey(itemId))
				_pack[itemId] = 0;
			_pack[itemId] += _cellHelper.Quantity(cell);

			_comparer.SmallToLarge = sortSmallToLarge;
			foreach (var entry in _pack)
				buffer.Add((entry.Key, entry.Value));
			buffer.Sort(_comparer);
			_pack.Clear();
		}

		public int GetAvailableSpace(InventoryEntity cell, ItemId itemId) {
			var item = _itemsDatabase.Get(itemId);

			var available = 0;

			if (!cell.IsBroken && (cell.IsEmpty || _cellHelper.Contains(cell, itemId)))
				available += item.StackSize - _cellHelper.Quantity(cell);

			return available;
		}

		public void Destroy(InventoryEntity cell) {
			if (cell.IsEmpty)
				return;

			_itemsFactory.Destroy(cell.CellTarget.Value);
			_cellHelper.Reset(cell);

			cell.IsChanged = true;
		}

		private sealed class PackedItemsComparer : IComparer<(ItemId, int)> {
			public bool SmallToLarge { get; set; }

			public int Compare((ItemId, int) x, (ItemId, int) y)
				=> SmallToLarge ? x.Item2.CompareTo(y.Item2) : y.Item2.CompareTo(x.Item2);
		}
	}
}