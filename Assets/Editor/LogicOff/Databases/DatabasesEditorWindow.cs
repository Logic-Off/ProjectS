using UnityEditor;
using UnityEngine;

namespace LogicOff.Databases {
	/// <summary>
	/// </summary>
	public sealed class DatabasesEditorWindow : EditorWindow {
		private static DatabaseEditorBuilder _currentWindow;

		[MenuItem("Window/LogicOff/Databases", priority = 100)]
		public static void Open() {
			var window = GetWindow<DatabasesEditorWindow>();
			window.titleContent = new GUIContent("Databases");
		}

		private void OnEnable() => _currentWindow = new DatabaseEditorBuilder(rootVisualElement);

		private void OnDisable() => _currentWindow = null;

		public static void Redraw() => _currentWindow?.Redraw();
	}
}