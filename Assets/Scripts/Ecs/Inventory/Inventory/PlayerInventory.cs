using System.Collections.Generic;
using Ecs.Item;
using Utopia;

namespace Ecs.Inventory {
	[InstallerGenerator(InstallerId.Game, 100_000, EInstallType.None, EBindType.BindInterfacesTo)]
	public class PlayerInventory : IPlayerInventory {
		private readonly List<EContainerType> _inventoryContainers = new() {EContainerType.Inventory};
		private List<InventoryEntity> _containerBuffer = new();
		private readonly List<InventoryEntity> _cellBuffer = new();

		private readonly InventoryContext _inventory;
		private readonly GameContext _game;
		private readonly IStackManipulator _stackManipulator;

		public PlayerInventory(
			InventoryContext inventory,
			GameContext game,
			IStackManipulator stackManipulator
		) {
			_inventory = inventory;
			_game = game;
			_stackManipulator = stackManipulator;
		}

		public bool Add(ItemId itemId, int quantity) {
			var result = _stackManipulator.Put(GetCells(), itemId, quantity);
			Refresh();
			return result;
		}

		public void Remove(ItemId itemId, int quantity) {
			_stackManipulator.Remove(GetCells(), itemId, quantity);
			Refresh();
		}

		public int Count(ItemId itemId) {
			var count = _stackManipulator.Count(GetCells(), itemId);
			return count;
		}

		public bool Has(ItemId itemId) => _stackManipulator.Count(GetCells(), itemId) > 0;

		public bool IsEnoughSpace(ItemId itemId, int quantity = 1) => _stackManipulator.IsEnoughSpace(GetCells(), itemId, quantity);

		public bool IsEnoughSpace(List<IItemData> items) => _stackManipulator.IsEnoughSpace(GetCells(), items);

		public ItemInstanceId Find(ItemId itemId) {
			var find = _stackManipulator.FindAnyInstanceOf(GetCells(), itemId);
			return find.result ? find.instanceId : ItemInstanceId.None;
		}

		public void FindAll(ItemId itemId, ref List<ItemInstanceId> buffer) {
			var result = _stackManipulator.FindAllInstancesOf(GetCells(), new[] {itemId}, buffer);
		}

		public void AllItems(ref List<ItemInstanceId> items) {
			var cells = GetCells();
			foreach (var cell in cells) {
				if (cell.IsEmpty)
					continue;
				items.Add(cell.CellTarget.Value);
			}
		}

		private void Refresh() {
			foreach (var container in GetContainers())
				container.IsChanged = true;
		}

		private List<InventoryEntity> GetCells() {
			_cellBuffer.Clear();

			foreach (var container in GetContainers()) {
				var cells = _inventory.GetEntitiesWithContainerOwner(container.ContainerId.Value);
				_cellBuffer.AddRange(cells);
			}

			return _cellBuffer;
		}

		private List<InventoryEntity> GetContainers() {
			_containerBuffer.Clear();

			foreach (var type in _inventoryContainers)
				_containerBuffer.Add(_inventory.GetContainerByType(_game.PlayerEntity.Id.Value, type));

			return _containerBuffer;
		}
	}
}