using UnityEditor;
using UnityEngine.UIElements;

namespace LogicOff.Toolbar {
	[InitializeOnLoad]
	public class ToolbarInitializer {
		private static bool _isInit;
		private static ToolbarBuilder _currentToolbar;

		static ToolbarInitializer() {
			_isInit = false;
			ToolbarCallback.OnToolbarGUIContainer -= OnUpdate;
			ToolbarCallback.OnToolbarGUIContainer += OnUpdate;
		}

		private static void OnUpdate(VisualElement container) {
			if (_isInit)
				return;

			_isInit = true;

			_currentToolbar = new ToolbarBuilder(container);
		}

		public static void Redraw() => _currentToolbar?.Redraw();
	}
}