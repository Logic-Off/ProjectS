using UnityEngine;
using UnityEngine.UI;

namespace Ecs.Ui {
	public sealed class InventoryCellWidget : ResourceCellWidget, IOnInteractableChangeEventListener {
		protected override EUiType Type => EUiType.InventoryCell;
		[SerializeField] private Image _interactableTarget;

		public override UiEntity Build(UiContext context, UiEntity parent) {
			var element = base.Build(context, parent);

			element.IsInteractable = _interactableTarget.raycastTarget;
			element.SubscribeOnInteractableChange(this);

			return element;
		}

		public void OnChangeInteractable(UiEntity entity) => _interactableTarget.raycastTarget = entity.IsInteractable;
	}
}