using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace LogicOff.Databases {
	/// <summary>
	/// Редакторы которые используются в текущей бд, должны наследоваться от этого интерфейса
	/// </summary>
	public interface ICustomEditor {
		string Name { get; }
		void AttachTo(VisualElement parent);
		void Detach();
	}

	public sealed class DatabasesEditorModel {
		private readonly Dictionary<string, ICustomEditor> _editors = new();

		private readonly DatabaseEditorPresenter _presenter;

		public DatabasesEditorModel(DatabaseEditorPresenter presenter) => _presenter = presenter;

		public void OnInitialize() => OnLoad();

		public void OnSelectedPrimeIndex(int value) => EditorPrefs.SetInt("DatabaseEditorCurrentType", value);

		public void OnSelectedPrimeObject(object value) => SetPrimeObject(value);

		public void OnSelectedSubIndex(int value) => EditorPrefs.SetInt("DatabaseEditorCurrentDatabase", value);

		public void OnSelectDatabase(object value) {
			Clear();
			_presenter.CurrentDatabase.Value = value as Object;
		}

		private void SetPrimeObject(object value) {
			if (value == null)
				return;

			SetSubObject(value.ToString());
			// Если нет бд, то это редактор
			if (_presenter.Databases.Value.Count == 0) {
				OnSelectedSubIndex(-1);

				return;
			}

			var index = GetCurrentDatabaseIndex();
			var databases = _presenter.Databases.Value;
			if (index == -1 && databases.Count > 0) {
				index = 0;
				OnSelectedSubIndex(0);
			}

			if (databases.Count == 0 || index == -1)
				return;
			OnSelectDatabase(databases.Count > index ? databases[index] : databases[0]);
		}

		// Добавляет редактор или второй столбец
		private void SetSubObject(string type) {
			var list = _presenter.Databases.Value;
			list.Clear();
			Clear();
			if (_editors.ContainsKey(type))
				AttachEditor(type);
			else
				list.AddRange(_presenter.MainObjects.Value[type].Values);

			_presenter.Databases.Value = list;
		}

		private void AttachEditor(string type) {
			var editor = _editors[type];
			_presenter.CurrentEditor.Value = editor;
			editor.AttachTo(_presenter.InfoContainer.Value);
		}

		private void Clear() {
			var editor = _presenter.CurrentEditor.Value;
			if (editor != null) {
				editor.Detach();
				_presenter.CurrentEditor.Value = null;
			}

			_presenter.CurrentDatabase.Value = null;
		}

		private void OnLoad() {
			var primeListElements = new List<string>();
			_presenter.MainObjects.Value.Clear();
			_editors.Clear();
			foreach (var entry in DatabasesPreferences.instance.Entries) {
				// Знаю что можно сразу создать объект, но по сути мне только путь надо дернуть, а другие методы уже сами решат что с этим делать
				var path = AssetDatabase.GetAssetPath(entry.Target);
				switch (entry.Type) {
					case EDatabasesSettingsEntryType.Asset:
						_presenter.MainObjects.Value.Add(entry.Name, DatabasesEditorExtensions.LoadDatabase(path));
						break;
					case EDatabasesSettingsEntryType.Folder:
						_presenter.MainObjects.Value.Add(
							entry.Name,
							DatabasesEditorExtensions.LoadDatabases<ScriptableObject>(path, entry.SearchOption)
						);
						break;
					case EDatabasesSettingsEntryType.Editor:
						var instance = (ICustomEditor)Activator.CreateInstance(entry.Domain, entry.ClassFullName).Unwrap();
						_editors.Add(entry.Name, instance);
						break;
					default:
						throw new ArgumentOutOfRangeException($"Невозможно создать объект с типом: {entry.Type}");
				}

				primeListElements.Add(entry.Name);
			}

			_presenter.PrimeListElements.Value = primeListElements;
			var type = EditorPrefs.GetInt("DatabaseEditorCurrentType", -1);
			var database = GetCurrentDatabaseIndex();
			if (type >= 0)
				_presenter.SelectedPrimeIndex.Value = type;
			if (database >= 0)
				_presenter.SelectedSubIndex.Value = database;
		}

		private int GetCurrentDatabaseIndex() => EditorPrefs.GetInt("DatabaseEditorCurrentDatabase", -1);
	}
}