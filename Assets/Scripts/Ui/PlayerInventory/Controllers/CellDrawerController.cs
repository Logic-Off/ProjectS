using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Ecs.Common;
using Ecs.Inventory;
using Ecs.Item;
using UnityEngine;
using Utopia;

namespace Ui.PlayerInventory {
	[InstallerGenerator(InstallerId.Ui)]
	public class CellDrawerController {
		private readonly ICellPool _cellPool;
		private readonly ILockedCellPool _lockedCellPool;
		private readonly InventoryContext _inventory;
		private readonly ICellHelper _cellHelper;

		public CellDrawerController(ICellPool cellPool, ILockedCellPool lockedCellPool, InventoryContext inventory, ICellHelper cellHelper) {
			_cellPool = cellPool;
			_lockedCellPool = lockedCellPool;
			_inventory = inventory;
			_cellHelper = cellHelper;
		}

		public async UniTask OnCreateCells(Id owner, Id panelId, Dictionary<CellId, Id> cells, EContainerType containerType, RectTransform transform) {
			var container = _inventory.GetContainerByType(owner, containerType);
			foreach (var cellId in container.Cells.Value) {
				var cell = await _cellPool.Get(panelId, cellId, transform);
				cell.ReplaceContainerType(containerType);
				UpdateUiCell(cell);
				cells[cellId] = cell.Id.Value;
			}
		}

		public void UpdateUiCell(UiEntity uiCell) {
			var cell = _inventory.GetEntityWithCellId(uiCell.TargetCellId.Value);
			UpdateUiCell(cell, uiCell);
		}

		public void UpdateUiCell(InventoryEntity cell, UiEntity uiCell) {
			var item = _cellHelper.GetItemId(cell);
			var iconId = item == ItemId.None ? $"EquipmentCell.Empty.{uiCell.ContainerType.Value}" : item.ToString();
			var color = item == ItemId.None ? new Color32(51, 103, 102, 255) : new Color32(255, 255, 255, 255);
			uiCell.ReplaceIconId(iconId);
			uiCell.ReplaceInt(_cellHelper.Quantity(cell));
			uiCell.ReplaceColor(color);
			uiCell.IsInteractable = !cell.IsEmpty;
			uiCell.IsEmpty = cell.IsEmpty;
		}
	}
}