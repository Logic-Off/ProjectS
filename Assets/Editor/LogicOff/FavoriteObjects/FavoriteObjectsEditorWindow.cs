using LogicOff.FavoriteObjects.Builders;
using UnityEditor;
using UnityEngine;

namespace LogicOff.FavoriteObjects {
	/// <summary>
	///   Author: Andrey Abramkin
	/// </summary>
	public sealed class FavoriteObjectsEditorWindow : EditorWindow {
		private FavoriteObjectsEditorBuilder _builder;

		private void OnEnable() {
			if (_builder != null)
				return;
			_builder = new FavoriteObjectsEditorBuilder(rootVisualElement);
		}

		[MenuItem("Tools/Editors/FavoriteObjects", priority = 0)]
		private static void Open() {
			var window = GetWindow<FavoriteObjectsEditorWindow>();
			window.titleContent = new GUIContent("FavoriteObjects");
		}
	}
}