using Common;
using Ecs.Item;
using Ecs.Ui;
using UnityEngine;
using Utopia;

namespace Ecs.Inventory {
	[InstallerGenerator(InstallerId.Game)]
	public class CellSwapProcessor : ICellSwapProcessor {
		private readonly InventoryContext _inventory;
		private readonly ItemContext _item;
		private readonly ItemPutChecker _itemPutChecker;
		private readonly ICellManipulator _manipulator;
		private readonly ICellHelper _cellHelper;
		private readonly UiContext _ui;

		public CellSwapProcessor(
			InventoryContext inventory,
			ItemContext item,
			ItemPutChecker itemPutChecker,
			ICellManipulator manipulator,
			ICellHelper cellHelper,
			UiContext ui
		) {
			_inventory = inventory;
			_item = item;
			_itemPutChecker = itemPutChecker;
			_manipulator = manipulator;
			_cellHelper = cellHelper;
			_ui = ui;
		}

		public void Swap(CellId departureId, CellId destinationId) {
			if (departureId == destinationId || departureId == CellId.None || destinationId == CellId.None)
				return;

			var departure = _inventory.GetEntityWithCellId(departureId);
			var destination = _inventory.GetEntityWithCellId(destinationId);

			// Перенос количества итемов
			var canMerge = CanMerge(departure, destination);
			if (canMerge) {
				var count = _manipulator.GetAvailableSpace(destination, _cellHelper.GetItemId(departure));
				var quantity = _cellHelper.Quantity(departure).Min(count);

				_cellHelper.Add(destination, quantity);
				_cellHelper.Remove(departure, quantity);
				SetCellContainerChanged(departure);
				SetCellContainerChanged(destination);
				return;
			}

			// Проверка на возможность переложить итем
			var swapResult = _itemPutChecker.CanSwapCells(departure, destination);
			if (!swapResult) {
#if DEBUG
				D.Error("[CellSwapProcessor]", $"Swap result: [{swapResult}]");
#endif
				return;
			}

			OnSwap(departure, destination);
		}

		public void Swap(InventoryEntity departure, InventoryEntity destination)
			=> Swap(departure.CellId.Value, destination.CellId.Value);

		private void OnSwap(InventoryEntity departure, InventoryEntity destination) {
			// ReSharper disable PossibleNullReferenceException
			var departureItem = _item.GetEntityWithCellId(departure.CellId.Value);
			var destinationItem = _item.GetEntityWithCellId(destination.CellId.Value);
			departureItem?.RemoveCellId();
			destinationItem?.RemoveCellId();

			OnSwap(departureItem, destination);
			OnSwap(destinationItem, departure);
			SetCellContainerChanged(departure);
			SetCellContainerChanged(destination);
			UpdateUiCell(departure);
			UpdateUiCell(destination);
		}

		private bool CanMerge(InventoryEntity from, InventoryEntity to) {
			var fromItemId = _cellHelper.GetItemId(from);
			var toItemId = _cellHelper.GetItemId(to);
			if (fromItemId != toItemId)
				return false;
			var fromItem = _item.GetEntityWithItemInstanceId(from.CellTarget.Value);
			return fromItem.StackSize.Value > 1;
		}

		private void OnSwap(ItemEntity item, InventoryEntity target) {
			if (item != null)
				_cellHelper.SetItem(target, item, item.Quantity.Value);
			else
				_cellHelper.Reset(target);
		}

		public void SetCellContainerChanged(InventoryEntity cell)
			=> _inventory.GetEntityWithContainerId(cell.ContainerOwner.Value).IsChanged = true;

		public void UpdateUiCell(InventoryEntity cell) {
			var allTargets = _ui.GetEntitiesWithTargetCellId(cell.CellId.Value);
			foreach (var uiEntity in allTargets) {
				if (uiEntity.UiType.Value is not EUiType.InventoryCell)
					continue;

				var item = _cellHelper.GetItemId(cell);
				var iconId = item == ItemId.None ? $"EquipmentCell.Empty.{uiEntity.ContainerType.Value}" : item.ToString();
				var color = item == ItemId.None ? new Color32(51, 103, 102, 255) : new Color32(255, 255, 255, 255);
				uiEntity.ReplaceIconId(iconId);
				uiEntity.ReplaceInt(_cellHelper.Quantity(cell));
				uiEntity.ReplaceColor(color);
				uiEntity.IsInteractable = !cell.IsEmpty;
				uiEntity.IsEmpty = cell.IsEmpty;
			}
		}
	}
}