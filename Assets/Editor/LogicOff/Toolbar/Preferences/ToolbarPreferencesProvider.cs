using System.Collections.Generic;
using System.IO;
using System.Reflection;
using LogicOff.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LogicOff.Toolbar {
	internal sealed class ToolbarPreferencesProvider : SettingsProvider {
		private SerializedObject _target;
		private SerializedProperty _entries;
		private SerializedProperty _defaultEntries;

		public ToolbarPreferencesProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null)
			: base(path, scopes, keywords) { }

		public override void OnActivate(string searchContext, VisualElement rootElement) {
			ToolbarPreferences.instance.Save();
			_target = ToolbarPreferences.instance.GetSerializedObject();
			_entries = _target.FindProperty("Entries");
			_defaultEntries = _target.FindProperty(nameof(ToolbarPreferences.DefaultEntries));
		}

		public override void OnGUI(string searchContext) {
			_target.Update();
			EditorGUI.BeginChangeCheck();

			GUI.enabled = false;
			EditorGUILayout.PropertyField(_defaultEntries, new GUIContent("Стандартные настройки(только чтение)"));
			GUI.enabled = true;
			EditorGUILayout.PropertyField(_entries, new GUIContent("Текущие настройки тулбара"));

			if (EditorGUI.EndChangeCheck()) {
				_target.ApplyModifiedProperties();
				ToolbarPreferences.instance.Save();
				ToolbarInitializer.Redraw();
			}

			if (GUILayout.Button("Collect scenes")) {
				ToolbarPreferences.instance.OnCollectScenes();
				_target.ApplyModifiedProperties();
				ToolbarPreferences.instance.Save();
				ToolbarInitializer.Redraw();
			}
		}

		internal class Styles { }

		[SettingsProvider]
		public static SettingsProvider CreateToolbarProjectSettingProvider() {
			var provider = new ToolbarPreferencesProvider("Preferences/Toolbar", SettingsScope.User, GetSearchKeywordsFromGUIContentProperties<Styles>());
			return provider;
		}
	}
}