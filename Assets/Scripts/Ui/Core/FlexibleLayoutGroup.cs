using Common;
using UnityEngine;
using UnityEngine.UI;

namespace Ui {
	[DisallowMultipleComponent]
	[ExecuteAlways]
	[RequireComponent(typeof(RectTransform))]
	public sealed class FlexibleLayoutGroup : MonoBehaviour, ILayoutElement {
		[SerializeField] private RectTransform _rect;
		[SerializeField] private RectOffset _padding = new();
		[SerializeField] private Vector2 _space;
		private bool _isChanged;
		[SerializeField] private bool _isPreferredHeight;
		[SerializeField] private bool _stretchChildHorizontally;

		private void Awake() => _rect = GetComponent<RectTransform>();

		private void OnEnable() => OnChange();
		private void OnTransformChildrenChanged() => OnChange();
		private void OnRectTransformDimensionsChange() => OnChange();
		private void OnDidApplyAnimationProperties() => OnChange();

#if UNITY_EDITOR
		private void OnValidate() {
			if (_rect == null)
				_rect = transform.GetComponent<RectTransform>();

			UpdatePosition();
		}
#endif

		private void OnChange() {
			// if (_isChanged || !isActiveAndEnabled)
			// 	return;
			//
			// if (Application.isPlaying)
			// 	_isChanged = true;
			// else
			// 	UpdatePosition();
		}

		private void Update() => UpdatePosition();

		[ContextMenu("Update")]
		private void UpdatePosition() {
			var size = _rect.rect.size;
			var startPosition = new Vector2(_padding.left, -_padding.top);
			var maxY = -_padding.top.ToFloat();

			var maxChildSizeY = 0f;

			for (var i = 0; i < transform.childCount; i++) {
				var child = _rect.GetChild(i);
				if (!child.gameObject.activeSelf)
					continue;
				var childRect = (RectTransform)child;
				childRect.anchorMin = Vector2.up;
				childRect.anchorMax = _stretchChildHorizontally ? Vector2.one : Vector2.up;
				childRect.pivot = Vector2.up;
				var childSize = childRect.rect.size;
				if (_stretchChildHorizontally) {
					childRect.sizeDelta = new Vector2(-_padding.left + -_padding.right, childRect.sizeDelta.y);
				}

				if (startPosition.x + childSize.x > size.x) {
					startPosition = new Vector3(_padding.left, startPosition.y + -(maxY + _space.y));
					maxY = -_padding.top;
					maxChildSizeY = 0;
				}

				if (maxY < childSize.y)
					maxY = childSize.y;

				childRect.anchoredPosition = startPosition;

				startPosition += new Vector2(childSize.x + _space.x, 0);
				if (maxChildSizeY < childSize.y)
					maxChildSizeY = childSize.y;
			}

			_isChanged = false;

			if (_isPreferredHeight) {
				preferredHeight = Mathf.Abs(startPosition.y) + maxChildSizeY + _padding.bottom;
				_rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredHeight);
			}
		}

		// Код для ContentSizeFilter
		public void CalculateLayoutInputHorizontal() { }

		public void CalculateLayoutInputVertical() { }

		public float minWidth { get; set; }
		public float preferredWidth { get; set; }
		public float flexibleWidth { get; set; }
		public float minHeight { get; }
		public float preferredHeight { get; set; }
		public float flexibleHeight { get; }
		public int layoutPriority { get; }
	}
}