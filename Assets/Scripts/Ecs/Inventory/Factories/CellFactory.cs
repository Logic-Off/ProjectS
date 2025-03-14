using Utopia;

namespace Ecs.Inventory {
	[InstallerGenerator(InstallerId.Game)]
	public sealed class CellFactory : ICellFactory {
		private readonly InventoryContext _inventory;
		private readonly CellIdGenerator _idGenerator;

		public CellFactory(InventoryContext inventory, CellIdGenerator idGenerator) {
			_inventory = inventory;
			_idGenerator = idGenerator;
		}

		public InventoryEntity Create(ContainerId containerId) {
			var cell = _inventory.CreateEntity();
			var cellId = _idGenerator.Next();
			cell.AddCellId(cellId);
			cell.AddContainerOwner(containerId);
			cell.IsChanged = true;
			cell.IsEmpty = true;
			cell.AddCellTarget(ItemInstanceId.None);

			return cell;
		}
	}
}