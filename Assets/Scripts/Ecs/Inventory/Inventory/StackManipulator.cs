using System.Collections.Generic;
using Ecs.Item;
using UnityEngine.Assertions;
using Utopia;

namespace Ecs.Inventory {
	[InstallerGenerator(InstallerId.Game)]
	public sealed class StackManipulator : IStackManipulator {
		private static Dictionary<ItemId, int> _pack = new();
		private static PackedItemsComparer _comparer = new();
		private static readonly List<InventoryEntity> Transaction = new(8);

		private readonly IItemsDatabase _itemsDatabase;
		private readonly IItemsFactory _itemsFactory;
		private readonly ICellHelper _cellHelper;

		public StackManipulator(
			IItemsDatabase itemsDatabase,
			IItemsFactory itemsFactory,
			ICellHelper cellHelper
		) {
			_itemsDatabase = itemsDatabase;
			_itemsFactory = itemsFactory;
			_cellHelper = cellHelper;
		}

		public bool Put(List<InventoryEntity> cells, ItemId itemId, int quantity) {
			Assert.IsTrue(quantity > 0, $"Amount less than 1 ({quantity})!");
			var commit = false;

			var item = _itemsDatabase.Get(itemId);
			Assert.IsTrue(item.StackSize > 0, "Stack size are 0");
			var referencedQuantity = quantity;
			
			var result = TryPutInUsedCell(item, cells, ref referencedQuantity);
			if (result) {
				commit = Commit(item, quantity);
				Transaction.Clear();
				return commit;
			}

			var quantityInEmptyCell = referencedQuantity;
			result = TryPutInEmptyCell(item, cells, ref referencedQuantity);
			if (result)
				commit = Commit(item, quantity - quantityInEmptyCell);
			Transaction.Clear();
			return result && commit;
		}

		public (bool allPut, int remainder) PutAny(List<InventoryEntity> cells, ItemId itemId, int quantity) {
			Assert.IsTrue(quantity > 0, "Amount less than 1!");

			var item = _itemsDatabase.Get(itemId);
			Assert.IsTrue(item.StackSize > 0, "Stack size are 0");
			var referencedQuantity = quantity;
			var result = TryPutInUsedCell(item, cells, ref referencedQuantity);

			Commit(item, quantity);
			Transaction.Clear();
			if (result)
				return (true, 0);

			var quantityInEmptyCell = referencedQuantity;
			result = TryPutInEmptyCell(item, cells, ref referencedQuantity);
			return (result, quantityInEmptyCell);
		}

		private bool Commit(ItemEntry item, int quantity) {
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

		private bool TryPutInUsedCell(ItemEntry item, List<InventoryEntity> cells, ref int quantity) {
			if (!item.IsStackable)
				return false;

			foreach (var cell in cells) {
				if (CellCantAcceptThatItem(cell, item))
					continue;

				var cellQuantity = _cellHelper.Quantity(cell);
				if (cellQuantity + quantity <= item.StackSize) {
					Transaction.Add(cell);
					return true;
				}

				var count = item.StackSize - cellQuantity;
				Transaction.Add(cell);
				quantity -= count;
			}

			return false;
		}

		private bool TryPutInEmptyCell(ItemEntry item, List<InventoryEntity> cells, ref int quantity) {
			foreach (var cell in cells) {
				if (cell.IsEmpty && !cell.IsBroken) {
					var instance = _itemsFactory.Create(item.Id);
					if (quantity > item.StackSize) {
						quantity -= item.StackSize;
						_cellHelper.SetItem(cell, instance, item.StackSize);
						continue;
					}

					_cellHelper.SetItem(cell, instance, quantity);
					return true;
				}
			}

			return false;
		}

		public void Remove(List<InventoryEntity> cells, ItemId itemId, int amount) {
			foreach (var cell in cells) {
				if (!_cellHelper.Contains(cell, itemId))
					continue;

				var quantity = _cellHelper.Quantity(cell);
				if (quantity >= amount) {
					_cellHelper.Remove(cell, amount);
					return;
				}

				_itemsFactory.Destroy(cell.CellTarget.Value);
				_cellHelper.Reset(cell);
				amount -= quantity;
			}
		}

		public void RemoveAll(List<InventoryEntity> cells, ItemId itemId) {
			foreach (var cell in cells) {
				if (!_cellHelper.Contains(cell, itemId))
					continue;
				_itemsFactory.Destroy(cell.CellTarget.Value);
				_cellHelper.Reset(cell);
			}
		}

		public bool IsEnoughSpace(List<InventoryEntity> cells, ItemId itemId, int amount) {
			Assert.IsTrue(amount > 0, "Amount less than 1!");

			var quantity = amount;

			var item = _itemsDatabase.Get(itemId);

			return CanFitInUsedCell(item, cells, ref quantity) ||
			       CanFitInEmptyCell(item, cells, ref quantity) ||
			       CanMerge(quantity, amount);
		}

		public bool IsEnoughSpace(List<InventoryEntity> cells, IReadOnlyList<IItemData> items) {
			var emptyCells = 0;
			foreach (var cell in cells)
				emptyCells += cell.IsEmpty && !cell.IsBroken ? 1 : 0;

			if (emptyCells >= items.Count)
				return true;

			foreach (var itemRecord in items) {
				Assert.IsTrue(itemRecord.Quantity > 0, "Amount less than 1!");

				var item = _itemsDatabase.Get(itemRecord.Id);
				var quantity = itemRecord.Quantity;
				// Если будут предметы одного типа, это может вызвать ошибку!
				// Т.е. может быть такое, что два предмета в списке попадут в одну ячейку с предметом
				if (CanFitInUsedCell(item, cells, ref quantity) || CanMerge(quantity, itemRecord.Quantity))
					continue;

				if (!CanFitInEmptyCell(item, cells, ref quantity))
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

		private bool CanFitInUsedCell(ItemEntry item, List<InventoryEntity> cells, ref int quantity) {
			if (!item.IsStackable)
				return false;

			var stackSize = item.StackSize;

			foreach (var cell in cells) {
				if (CellCantAcceptThatItem(cell, item))
					continue;

				var cellQuantity = _cellHelper.Quantity(cell);
				if (cellQuantity + quantity <= stackSize)
					return true;

				quantity -= stackSize - cellQuantity;
			}

			return false;
		}

		private static bool CanFitInEmptyCell(ItemEntry item, List<InventoryEntity> cells, ref int quantity) {
			foreach (var cell in cells) {
				if (!cell.IsEmpty || cell.IsBroken)
					continue;
				if (quantity > item.StackSize)
					quantity -= item.StackSize;
				else
					return true;
			}

			return false;
		}

		private bool CellCantAcceptThatItem(InventoryEntity cell, ItemEntry item)
			=> _cellHelper.Quantity(cell) >= item.StackSize || !_cellHelper.Contains(cell, item.Id);

		public int Count(List<InventoryEntity> cells, ItemId itemId) {
			var sum = 0;
			foreach (var cell in cells)
				if (_cellHelper.Contains(cell, itemId))
					sum += _cellHelper.Quantity(cell);

			return sum;
		}

		public bool Contains(List<InventoryEntity> cells, ItemId itemId) {
			foreach (var cell in cells)
				if (_cellHelper.Contains(cell, itemId))
					return true;

			return false;
		}

		public (bool, ItemId) ContainsAnyOf(List<InventoryEntity> cells, IEnumerable<ItemId> items) {
			foreach (var item in items)
			foreach (var cell in cells)
				if (_cellHelper.Contains(cell, item))
					return (true, item);
			return (false, default);
		}

		public (bool, ItemInstanceId) FindAnyInstanceOf(List<InventoryEntity> cells, ItemId item) {
			foreach (var cell in cells)
				if (_cellHelper.Contains(cell, item))
					return (true, cell.CellTarget.Value);
			return (false, default);
		}

		public (bool, ItemInstanceId) FindAnyInstanceOf(List<InventoryEntity> cells, IEnumerable<ItemId> items) {
			foreach (var item in items)
			foreach (var cell in cells)
				if (_cellHelper.Contains(cell, item))
					return (true, cell.CellTarget.Value);
			return (false, default);
		}

		public bool FindAllInstancesOf(List<InventoryEntity> cells, IEnumerable<ItemId> items, List<ItemInstanceId> buffer) {
			buffer.Clear();
			foreach (var item in items)
			foreach (var cell in cells)
				if (_cellHelper.Contains(cell, item))
					buffer.Add(cell.CellTarget.Value);
			return buffer.Count > 0;
		}

		public void PackedItems(List<InventoryEntity> cells, List<(ItemId, int)> buffer, bool sortSmallToLarge = true) {
			foreach (var cell in cells) {
				if (cell.IsEmpty)
					continue;
				var itemId = _cellHelper.GetItemId(cell);
				if (!_pack.ContainsKey(itemId))
					_pack[itemId] = 0;
				_pack[itemId] += _cellHelper.Quantity(cell);
			}

			_comparer.SmallToLarge = sortSmallToLarge;
			foreach (var entry in _pack)
				buffer.Add((entry.Key, entry.Value));
			buffer.Sort(_comparer);
			_pack.Clear();
		}

		public int GetAvailableSpace(List<InventoryEntity> cells, ItemId itemId) {
			var item = _itemsDatabase.Get(itemId);

			var available = 0;

			foreach (var cell in cells) {
				if (!cell.IsBroken && (cell.IsEmpty || _cellHelper.Contains(cell, itemId)))
					available += item.StackSize - _cellHelper.Quantity(cell);
			}

			return available;
		}

		public bool Split(List<InventoryEntity> cells, InventoryEntity targetCell) {
			foreach (var cell in cells)
				if (cell.IsEmpty && !cell.IsBroken) {
					var removeCount = _cellHelper.Quantity(targetCell) / 2;
					var targetItem = _cellHelper.GetItemId(targetCell);
					_cellHelper.Remove(targetCell, removeCount);
					var item = _itemsFactory.Create(targetItem);
					_cellHelper.SetItem(cell, item, removeCount);
					return true;
				}

			return false;
		}

		private sealed class PackedItemsComparer : IComparer<(ItemId, int)> {
			public bool SmallToLarge { get; set; }

			public int Compare((ItemId, int) x, (ItemId, int) y)
				=> SmallToLarge ? x.Item2.CompareTo(y.Item2) : y.Item2.CompareTo(x.Item2);
		}
	}
}