using ObjectFinderEditor.Scripts.Databases;
using UnityEditor;
using UnityEngine;

namespace ObjectFinderEditor.Scripts {
	/// <summary>
	///
	/// </summary>
	[CustomPropertyDrawer(typeof(ObjectFinderScriptableObject.GuidEntry))]
	public class GuidEntryPropertyDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUILayout.BeginHorizontal();
			var value = property.FindPropertyRelative("Value");
			value.stringValue = EditorGUILayout.TextField(label, value.stringValue);
			if (GUILayout.Button("Find")) {
				var guid = value.stringValue;
				var path = AssetDatabase.GUIDToAssetPath(guid);
				var target = AssetDatabase.LoadAssetAtPath<Object>(path);
				if (target == null) {
					Debug.LogError($"Object with guid '{guid}' don't found");
				} else {
					Selection.activeObject = target;
					Debug.Log($"Target path: {path}");
				}
			}
			EditorGUILayout.EndHorizontal();
		}
	}
}