using UnityEngine;

namespace Common {
	public sealed class FpsBehaviour : MonoBehaviour {
		private float _currentFps;
		private int _frames;
		private float _accumulator;
		private float _timeLeft;
		private string _fpsText;

		private void Awake() {
			DontDestroyOnLoad(gameObject);
		}

		private void OnGUI() {
			Refresh();
			if (_timeLeft <= 0f) {
				Evaluate();
				_fpsText = $"FPS: {_currentFps.ToInt()}";
			}

			GUILayout.Label(_fpsText);
		}

		private void Refresh() {
			var unscaledDeltaTime = Time.unscaledDeltaTime;
			_frames++;
			_accumulator += unscaledDeltaTime;
			_timeLeft -= unscaledDeltaTime;
		}

		private void Evaluate() {
			_currentFps = _accumulator > 0f ? _frames / _accumulator : -1f;
			_frames = 0;
			_accumulator = 0f;
			_timeLeft += .3f;
		}
	}
}