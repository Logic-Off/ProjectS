using System;
using UnityEngine;

namespace Common {
	public sealed class MonoGizmosBehaviour : MonoBehaviour {
		private Action _drawGizmoCallback;

		public void Subscribe(Action callback) => _drawGizmoCallback = callback;

		private void OnDrawGizmos() => _drawGizmoCallback?.Invoke();

		private void OnDestroy() => _drawGizmoCallback = null;
	}
}