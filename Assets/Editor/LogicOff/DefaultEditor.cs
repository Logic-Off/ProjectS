using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace LogicOff {
	/// <summary>
	/// Обертка для стандартного IMGUI редактора
	/// </summary>
	public static class DefaultEditor {
		public static void FillDefaultInspector(
			VisualElement container,
			SerializedObject serializedObject,
			bool hideScript
		) {
			SerializedProperty property = serializedObject.GetIterator();
			if (property.NextVisible(true)) {
				do {
					if (property.propertyPath == "m_Script" && hideScript)
						continue;

					var field = new PropertyField(property) { name = "PropertyField:" + property.propertyPath };
					field.RegisterValueChangeCallback(x => serializedObject.ApplyModifiedProperties());
					if (property.propertyPath == "m_Script" && serializedObject.targetObject != null)
						field.SetEnabled(false);

					container.Add(field);
				} while (property.NextVisible(false));
			}
		}

		public static void FillDefaultInspectorIMGUI(
			VisualElement visualElement,
			SerializedObject serializedObject,
			Action<SerializedProperty> callbackProperty = null
		) {
			var container = new IMGUIContainer(() => OnGUI(serializedObject, callbackProperty));
			visualElement.Add(container);
		}

		private static void OnGUI(SerializedObject serializedObject, Action<SerializedProperty> callbackProperty = null) {
			SerializedProperty property = serializedObject.GetIterator();
			if (property.NextVisible(true)) {
				do {
					EditorGUI.BeginChangeCheck();
					callbackProperty?.Invoke(property);
					EditorGUILayout.PropertyField(property, true);
					if (EditorGUI.EndChangeCheck())
						serializedObject.ApplyModifiedProperties();
				} while (property.NextVisible(false));
			}
		}
	}
}