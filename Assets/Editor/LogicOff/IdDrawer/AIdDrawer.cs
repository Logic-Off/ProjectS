using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public abstract class AIdDrawer<TDataBase, TValue> : PropertyDrawer where TDataBase : class {
	private static IdDropdownDrawer _drawer;
	protected static TDataBase Database;
	protected static string[] Values;

	protected abstract string Name { get; }
	protected abstract string Path { get; }

	protected static TDataBase Load(string path) {
		var assets = AssetDatabase.LoadAssetAtPath(path, typeof(TDataBase));
		var database = assets as TDataBase;

		return database ?? throw new Exception($"{typeof(TDataBase)} was not found at build-in path!");
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		if (Database == null)
			Refresh();

		DrawPopup(label.text, Values, position, property);
	}

	protected void DrawPopup(string name, string[] items, Rect position, SerializedProperty property) {
		var valueProperty = property.FindPropertyRelative("_value");
		var previous = Array.IndexOf(items, valueProperty.stringValue);

		position.x += 2;

		if (name.IsNotNullOrEmpty()) {
			EditorGUI.LabelField(position, name);
			position.x += 100;
			position.width -= 100;
		}

		if (previous == -1) {
			position.width -= 65;
			if (GUI.Button(position, valueProperty.stringValue)) {
				var rect = GUILayoutUtility.GetRect(new GUIContent("Show"), EditorStyles.toolbarButton);
				rect.position = Event.current.mousePosition;
				_drawer.Property = valueProperty;
				_drawer.Show(rect);
			}

			position.x += position.width + 5;
			position.width = 60;

			if (GUI.Button(position, "Refresh"))
				Refresh();
		} else {
			if (GUI.Button(position, valueProperty.stringValue)) {
				var rect = GUILayoutUtility.GetRect(new GUIContent("Show"), EditorStyles.toolbarButton);
				rect.position = Event.current.mousePosition;
				_drawer.Property = valueProperty;
				_drawer.Show(rect);
			}
		}
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 16f;

	private static string[] GetNames<T>(int length, IEnumerable<T> list, Func<T, string> getter) {
		var result = new string[length];
		var i = 0;
		foreach (var value in list)
			result[i++] = getter(value);
		return result;
	}

	protected virtual void Refresh() {
		Database = Load(Path);
		var array = GetList(Database).ToArray();
		var list = GetNames(array.Length, array, GetNameFromItem).ToList();
		list.Sort();
		Values = list.ToArray();
		if (_drawer == null)
			_drawer = new IdDropdownDrawer(new AdvancedDropdownState(), Name);
		_drawer.Items = Values;
	}

	protected abstract IEnumerable<TValue> GetList(TDataBase database);
	protected abstract string GetNameFromItem(TValue value);
}