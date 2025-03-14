using System;
using System.Collections.Generic;
using Ui;
using UnityEngine.EventSystems;

namespace Ecs.Ui {
	public class DragAndDropWidget : AWidget, IDisposable, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler,
		IPointerDownHandler, IPointerExitHandler {
		protected override EUiType Type => EUiType.DragAndDrop;
		protected UiEntity _entity;

		public override UiEntity Build(UiContext ui, UiEntity parent) {
			_entity = base.Build(ui, parent);
			_entity.AddTouchEvents(new List<TouchEvent>());
			_entity.AddOnTouchSubscribers(new List<Action<UiEntity, TouchEvent>>());
			return _entity;
		}

		public virtual void OnDrag(PointerEventData eventData) => SetData(eventData, ETouchState.Drag);
		public virtual void OnBeginDrag(PointerEventData eventData) => SetData(eventData, ETouchState.BeginDrag);
		public virtual void OnEndDrag(PointerEventData eventData) => SetData(eventData, ETouchState.EndDrag);
		public virtual void OnPointerUp(PointerEventData eventData) => SetData(eventData, ETouchState.Released);
		public virtual void OnPointerDown(PointerEventData eventData) => SetData(eventData, ETouchState.Pressed);
		public virtual void OnPointerClick(PointerEventData eventData) => SetData(eventData, ETouchState.Click);
		public virtual void OnPointerExit(PointerEventData eventData) => SetData(eventData, ETouchState.Exit);

		protected virtual void SetData(PointerEventData eventData, ETouchState state) {
			// if (state is ETouchState.BeginDrag or ETouchState.Drag)
			// return;
			_entity.TouchEvents.List.Add(
				new TouchEvent {
					State = state,
					Position = eventData.position,
					Delta = eventData.delta,
					Finger = eventData.pointerId,
					Target = eventData.pointerEnter != null ? eventData.pointerEnter.gameObject : null
				}
			);
			_entity.ReplaceTouchEvents(_entity.TouchEvents.List);
		}

		public void Dispose() => _entity = null;
	}
}