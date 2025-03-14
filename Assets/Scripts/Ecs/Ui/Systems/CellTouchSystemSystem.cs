using System;
using System.Collections.Generic;
using Ecs.Inventory;
using Ui.Draggable;
using UnityEngine;
using Utopia;
using Zentitas;

namespace Ecs.Ui {
	[InstallerGenerator(InstallerId.Ui)]
	public class CellTouchSystemSystem : ReactiveSystem<UiEntity> {
		private readonly UiContext _ui;
		private readonly DraggablePresenter _draggablePresenter;
		private readonly ICellSwapProcessor _cellSwapProcessor;

		public CellTouchSystemSystem(UiContext ui, DraggablePresenter draggablePresenter, ICellSwapProcessor cellSwapProcessor) : base(ui) {
			_ui = ui;
			_draggablePresenter = draggablePresenter;
			_cellSwapProcessor = cellSwapProcessor;
		}

		protected override ICollector<UiEntity> GetTrigger(IContext<UiEntity> context)
			=> context.CreateCollector(UiMatcher.TouchEvents.Added());

		protected override bool Filter(UiEntity entity)
			=> entity.HasTouchEvents && entity.UiType.Value == EUiType.InventoryCell;

		protected override void Execute(List<UiEntity> entities) {
			foreach (var ui in entities) {
				foreach (var touchEvent in ui.TouchEvents.List) {
					switch (touchEvent.State) {
						case ETouchState.Released:
							ui.ReplaceVector2(Vector2.zero);
							break;
						case ETouchState.Pressed:
							break;
						case ETouchState.Click:
							ui.IsClicked = true;
							break;
						case ETouchState.BeginDrag:
							_draggablePresenter.IsVisible.Value = true;
							_draggablePresenter.IconId.Value = ui.IconId.Value;
							_draggablePresenter.Position.Value = touchEvent.Position;
							SetInteractableEmptyCells(true);
							break;
						case ETouchState.Drag:
							_draggablePresenter.Position.Value = touchEvent.Position;
							break;
						case ETouchState.EndDrag:
							var firstCellId = ui.HasTargetCellId ? ui.TargetCellId.Value : CellId.None;
							var secondCell = touchEvent.Target == null ? null : _ui.GetEntityWithInstanceId(touchEvent.Target.GetInstanceID());
							var secondCellId = secondCell == null ? CellId.None : secondCell.TargetCellId.Value;
							_cellSwapProcessor.Swap(firstCellId, secondCellId);
							_draggablePresenter.IsVisible.Value = false;
							SetInteractableEmptyCells(false);
							break;
						case ETouchState.Exit:
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}
			}
		}

		private void SetInteractableEmptyCells(bool isActive) {
			var cells = _ui.GetEntitiesWithUiType(EUiType.InventoryCell);
			foreach (var cell in cells)
				if (cell.IsEmpty)
					cell.IsInteractable = isActive;
		}
	}
}