using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LogicOff.Toolbar {
	public static class ToolbarCallback {
		private static readonly BindingFlags _flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
		private static readonly Type _toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");
		private static readonly Type _guiViewType = typeof(Editor).Assembly.GetType("UnityEditor.GUIView");

		private static readonly PropertyInfo _viewVisualTree = _guiViewType.GetProperty("visualTree", _flags);
		private static readonly FieldInfo _imguiContainerOnGui = typeof(IMGUIContainer).GetField("m_OnGUIHandler", _flags);
		private static ScriptableObject _currentToolbar;
		private static VisualElement _container;

		public static Action OnToolbarGUI;
		public static Action<VisualElement> OnToolbarGUIContainer;

		static ToolbarCallback() {
			EditorApplication.update -= OnUpdate;
			EditorApplication.update += OnUpdate;
		}

		private static void OnUpdate() {
			if (_currentToolbar != null)
				return;

			var toolbars = Resources.FindObjectsOfTypeAll(_toolbarType);
			_currentToolbar = toolbars.Length > 0 ? (ScriptableObject)toolbars[0] : null;
			if (_currentToolbar == null)
				return;

			var visualTree = (VisualElement)_viewVisualTree.GetValue(_currentToolbar, null);
			_container = (IMGUIContainer)visualTree[0];

			var handler = (Action)_imguiContainerOnGui.GetValue(_container);
			handler -= OnGUI;
			handler += OnGUI;
			_imguiContainerOnGui.SetValue(_container, handler);
		}

		private static void OnGUI() {
			OnToolbarGUI?.Invoke();
			OnToolbarGUIContainer?.Invoke(_container.parent);
		}
	}
}