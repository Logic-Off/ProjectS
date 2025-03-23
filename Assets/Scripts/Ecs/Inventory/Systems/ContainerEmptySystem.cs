using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.Inventory {
	[InstallerGenerator(InstallerId.Game)]
	public class ContainerEmptySystem : ReactiveSystem<InventoryEntity> {
		private readonly InventoryContext _inventory;
		public ContainerEmptySystem(InventoryContext inventory) : base(inventory) => _inventory = inventory;

		protected override ICollector<InventoryEntity> GetTrigger(IContext<InventoryEntity> context)
			=> context.CreateCollector(InventoryMatcher.Changed.Added());

		protected override bool Filter(InventoryEntity entity)
			=> entity.IsChanged && entity.HasCells;

		protected override void Execute(List<InventoryEntity> entities) {
			foreach (var entity in entities)
				entity.IsEmpty = CheckCells(entity.Cells.Value);
		}

		private bool CheckCells(List<CellId> ids) {
			foreach (var cellId in ids) {
				var cell = _inventory.GetEntityWithCellId(cellId);
				if (cell.IsEmpty)
					continue;

				return false;
			}

			return true;
		}
	}
}