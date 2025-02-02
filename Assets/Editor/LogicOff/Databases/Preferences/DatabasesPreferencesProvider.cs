using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LogicOff.Databases {
	internal sealed class DatabasesPreferencesProvider : SettingsProvider {
		private SerializedObject _target;
		private SerializedProperty _entries;
		private SerializedProperty _defaultEntries;

		public DatabasesPreferencesProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null)
			: base(path, scopes, keywords) { }

		public override void OnActivate(string searchContext, VisualElement rootElement) {
			DatabasesPreferences.instance.Save();
			_target = DatabasesPreferences.instance.GetSerializedObject();
			_entries = _target.FindProperty("Entries");
			_defaultEntries = _target.FindProperty("DefaultEntries");
		}

		public override void OnGUI(string searchContext) {
			_target.Update();
			EditorGUI.BeginChangeCheck();

			GUI.enabled = false;
			EditorGUILayout.PropertyField(_defaultEntries, new GUIContent("Стандартные настройки(только чтение)"));
			GUI.enabled = true;
			EditorGUILayout.PropertyField(_entries, new GUIContent("Текущие настройки"));

			if (EditorGUI.EndChangeCheck()) {
				_target.ApplyModifiedProperties();
				DatabasesPreferences.instance.Save();
				DatabasesEditorWindow.Redraw();
			}
		}

		internal class Styles { }

		[SettingsProvider]
		public static SettingsProvider CreateDatabasesProjectSettingProvider() {
			var provider = new DatabasesPreferencesProvider("Preferences/Databases", SettingsScope.User, GetSearchKeywordsFromGUIContentProperties<Styles>());
			return provider;
		}
	}
}