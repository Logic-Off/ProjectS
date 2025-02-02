using UnityEditor;
using UnityEngine;

namespace ObjectFinderEditor.Scripts {
	/// <summary>
	///
	/// </summary>
	public sealed class ObjectFinderEditorWindow : EditorWindow {
		[MenuItem("Tools/Editors/ObjectFinder", priority = 0)]
		public static void Open() {
			var window = GetWindow<ObjectFinderEditorWindow>();
			window.titleContent = new GUIContent("ObjectFinder");
		}

		private void OnEnable() {
			var builder = new ObjectFinderEditorDataBuilder();
			builder.AttachTo(rootVisualElement);
		}
	}
}