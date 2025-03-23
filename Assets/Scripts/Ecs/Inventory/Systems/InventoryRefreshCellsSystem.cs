using System.Collections.Generic;
using Ecs.Ui;
using Ui.PlayerInventory;
using Utopia;
using Zentitas;

namespace Ecs.Inventory {
	[InstallerGenerator(InstallerId.Game, 5_000)]
	public class InventoryRefreshCellsSystem : ReactiveSystem<InventoryEntity> {
		private readonly CellDrawerController _drawerController;
		private readonly UiContext _ui;

		public InventoryRefreshCellsSystem(
			InventoryContext inventory,
			CellDrawerController drawerController,
			UiContext ui
		) : base(inventory) {
			_drawerController = drawerController;
			_ui = ui;
		}

		protected override ICollector<InventoryEntity> GetTrigger(IContext<InventoryEntity> context)
			=> context.CreateCollector(InventoryMatcher.Changed.Added());

		protected override bool Filter(InventoryEntity entity)
			=> entity.HasCellId;

		protected override void Execute(List<InventoryEntity> entities) {
			foreach (var entity in entities) {
				var cellId = entity.CellId.Value;
				var uiEntities = _ui.GetEntitiesWithTargetCellId(cellId);
				foreach (var uiEntity in uiEntities) {
					if (uiEntity.UiType.Value is not EUiType.InventoryCell)
						continue;

					_drawerController.UpdateUiCell(entity, uiEntity);
				}
			}
		}
	}
}