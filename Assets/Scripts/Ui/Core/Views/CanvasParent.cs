using UnityEngine;

namespace Ui {
	public sealed class CanvasParent : MonoBehaviour, ICanvasParent {
		[SerializeField] private Canvas _canvas;
		[SerializeField] private Canvas _disabledCanvas;
		public Canvas Canvas => _canvas;
		public Canvas DisabledCanvas => _disabledCanvas;
	}
}