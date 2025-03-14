using System;
using Ui;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ecs.Ui {
	public sealed class JoystickWidget : AWidget, IPointerDownHandler, IDragHandler, IPointerUpHandler, IDisposable {
		private UiEntity _entity;
		protected override EUiType Type => EUiType.Joystick;

		[Header("Rect References")] public RectTransform containerRect;
		public RectTransform handleRect;

		[Header("Settings")] public float joystickRange = 50f;

		public override UiEntity Build(UiContext context, UiEntity parent) {
			_entity = base.Build(context, parent);
			return _entity;
		}

		public void Dispose() => _entity = null;

		private void Start() => SetupHandle();

		private void SetupHandle() {
			if (handleRect)
				UpdateHandleRectPosition(Vector2.zero);
		}

		public void OnPointerDown(PointerEventData eventData) => OnDrag(eventData);

		public void OnDrag(PointerEventData eventData) {
			RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out var position);
			position = ApplySizeDelta(position);

			var clampedPosition = ClampValuesToMagnitude(position);

			OutputPointerEventValue(position.normalized /* * clampedPosition.magnitude*/);

			if (handleRect)
				UpdateHandleRectPosition(clampedPosition * joystickRange);
		}

		public void OnPointerUp(PointerEventData eventData) {
			OutputPointerEventValue(Vector2.zero);

			if (handleRect)
				UpdateHandleRectPosition(Vector2.zero);
		}

		private void OutputPointerEventValue(Vector2 pointerPosition) => _entity.ReplaceVector2(pointerPosition);

		private void UpdateHandleRectPosition(Vector2 newPosition) => handleRect.anchoredPosition = newPosition;

		private Vector2 ApplySizeDelta(Vector2 position) {
			var sizeDelta = containerRect.sizeDelta;
			var x = position.x / sizeDelta.x * 2.5f;
			var y = position.y / sizeDelta.y * 2.5f;
			return new Vector2(x, y);
		}

		private Vector2 ClampValuesToMagnitude(Vector2 position) => Vector2.ClampMagnitude(position, 1);
	}
}