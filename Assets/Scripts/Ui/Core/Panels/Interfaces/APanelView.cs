using Ui.Interfaces;
using UnityEngine;

namespace Ui.Core.Panels {
	public abstract class APanelView : MonoBehaviour, IPanelView {
		private GameObject _gameObjectCache;
		private Transform _transformCache;
		private RectTransform _rectTransformCache;

		protected GameObject _gameObject {
			get {
				if (_gameObjectCache == null)
					_gameObjectCache = gameObject;
				return _gameObjectCache;
			}
		}

		protected Transform _transform {
			get {
				if (_transformCache == null)
					_transformCache = transform;
				return _transformCache;
			}
		}

		protected RectTransform _rectTransform {
			get {
				if (_rectTransformCache == null)
					_rectTransformCache = transform.GetComponent<RectTransform>();
				return _rectTransformCache;
			}
		}
	}
}