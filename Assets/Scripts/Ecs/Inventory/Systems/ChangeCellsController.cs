using Ecs.Common;
using Ecs.Item;
using Utopia;

namespace Ecs.Inventory {
	[InstallerGenerator(InstallerId.Game)]
	public class ChangeCellsController : IChangeCellsController {
		private readonly ICellFactory _cellFactory;
		private readonly InventoryContext _inventory;

		public ChangeCellsController(ICellFactory cellFactory, InventoryContext inventory) {
			_cellFactory = cellFactory;
			_inventory = inventory;
		}

		public void OnChangeCellSettings(Id owner, EContainerType containerType) {
			var container = _inventory.GetContainerByType(owner, containerType);
			OnChangeCellSettings(container);
		}

		public void OnChangeCellSettings(InventoryEntity container) {
			var cellsCount = container.CellSettings.Values[ECellType.Cell];

			// Количество ячеек не изменилось
			if (container.Cells.Value.Count == cellsCount)
				return;

			for (var i = container.CellsPool.Value.Count; i < cellsCount; i++) {
				var cell = _cellFactory.Create(container.ContainerId.Value);
				container.CellsPool.Value.Add(cell.CellId.Value);
			}

			var cells = container.Cells.Value;
			cells.Clear();

			for (var i = 0; i < cellsCount; i++)
				cells.Add(container.CellsPool.Value[i]);

			container.ReplaceCells(cells);
			container.IsChanged = true;
		}
	}
}