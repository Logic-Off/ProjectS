using UnityEngine.EventSystems;

namespace Ecs.Ui {
	public sealed class PressedButtonWidget : ButtonWidget, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
		private UiEntity _entity;

		public override UiEntity Build(UiContext context, UiEntity parent) {
			var element = base.Build(context, parent);
			_entity = element;
			return element;
		}

		private void OnDestroy() => _entity = null;

		public void OnPointerDown(PointerEventData eventData) => _entity.IsPressed = true;

		public void OnPointerUp(PointerEventData eventData) => _entity.IsPressed = false;

		public void OnPointerExit(PointerEventData eventData) => _entity.IsPressed = false;
	}
}