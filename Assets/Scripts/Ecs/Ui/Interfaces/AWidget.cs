using System.Collections.Generic;
using Common;
using Ecs.Common;
using Ui;
using UnityEngine;

namespace Ecs.Ui {
	public abstract class AWidget : MonoBehaviour {
		private GameObject _gameObjectCache;

		protected GameObject _gameObject {
			get {
				if (_gameObjectCache == null)
					_gameObjectCache = gameObject;
				return _gameObjectCache;
			}
		}

		protected abstract EUiType Type { get; }
		[SerializeField] protected string _name;
		[SerializeField] protected RectTransform _rect;
		[SerializeField] private List<ASubWidget> _subWidgets;

		public virtual UiEntity Build(UiContext context, UiEntity parent) {
			var element = context.CreateEntity();
			var name = _name.IsNullOrEmpty() ? _gameObject.name : _name;
			element.AddId(IdGenerator.GetNext());
			element.AddName($"{parent.Name.Value}.{name}");
			element.AddParent(parent.Id.Value);
			element.AddUiType(Type);
			element.IsVisible = _gameObject.activeSelf;
			element.IsActive = parent.IsActive;
			element.SubscribeOnActiveChange(OnChangeActive);
			element.SubscribeOnVisibleChange(OnChangeVisible);
			element.AddRect(_rect);
			element.AddInstanceId(_gameObject.GetInstanceID());

			foreach (var child in _subWidgets)
				child.Build(context, parent, element);
			return element;
		}

		public void OnChangeActive(UiEntity entity) => OnChangeVisible(entity);
		public void OnChangeVisible(UiEntity entity) => _gameObject.SetActive(entity.IsVisible && entity.IsActive);

		protected virtual void OnValidate() {
			if (_rect == null) {
				_rect = _gameObject.GetComponent<RectTransform>();
#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(_gameObject);
#endif
			}
		}
	}
}