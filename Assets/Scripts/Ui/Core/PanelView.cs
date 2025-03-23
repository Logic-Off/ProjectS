using System;
using System.Collections.Generic;
using Common;
using Ecs.Ui;
using Sirenix.OdinInspector;
using Ui.Core.Panels;
using UnityEngine;

namespace Ui {
	public sealed class PanelView : APanelView, IOnSiblingChangeEventListener {
		[SerializeField] private List<AWidget> _widgets;
		[SerializeField] private AnimationUI _animation;

		private UiEntity _entity;
		private bool _previousState;

		private void OnDestroy() => _entity = null;

#if UNITY_EDITOR
		[Button]
		public void CollectWidgets() {
			_widgets.Clear();
			var widgets = _gameObject.GetComponentsInChildren<AWidget>(true);
			_widgets.AddRange(widgets);
			UnityEditor.EditorUtility.SetDirty(_gameObject);
		}
#endif

		public void Build(UiContext context, UiEntity entity) {
			_entity = entity;

			entity.AddUiType(EUiType.Panel);
			entity.AddRect(_rectTransform);
			entity.SubscribeOnActiveChange(OnChangeActive);
			entity.SubscribeOnVisibleChange(OnChangeVisible);
			entity.SubscribeOnSiblingChange(this);

			foreach (var element in _widgets) {
				if (element == null)
					throw new NullReferenceException($"Null widget at [{name}]");
				element.Build(context, entity);
			}

			OnChangeVisible(entity);
		}

		public void OnChangeActive(UiEntity entity) {
			OnChangeVisible(entity);

			if (_previousState == entity.IsActive)
				return;
			_previousState = entity.IsActive;

			if (_animation == null)
				OnEndAnimation();
			else
				OnAnimation(entity);
		}

		private void OnAnimation(UiEntity entity) {
			if (entity.IsActive) {
				_animation.OnAnimationEnded = OnEndAnimation; // Думаю надо добавить вечный ивент, а не каждый раз присваивать
				_animation.Play();
			} else {
				_animation.PreviewStart();
			}
		}

		public void OnEndAnimation() => _entity.IsAnimated = false;

		public void OnChangeVisible(UiEntity entity) => _gameObject.SetActive(entity.IsVisible && entity.IsActive);

		public void OnChangeSibling(UiEntity entity) {
			switch (entity.Sibling.Value) {
				case ESibling.First:
					_transform.SetAsFirstSibling();
					break;
				case ESibling.Last:
					_transform.SetAsLastSibling();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}