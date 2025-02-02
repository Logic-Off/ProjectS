using LogicOff.FavoriteObjects.Databases;
using LogicOff.FavoriteObjects.Presenters;
using UnityEditor;
using UnityEngine;

namespace LogicOff.FavoriteObjects.Models {
	/// <summary>
	///   Author: Andrey Abramkin
	/// </summary>
	public sealed class FavoriteObjectsEditorModel {
		private readonly string _defaultPathSettings =
			"Assets/LogicOff/Editor/FavoriteObjects/Settings/FavoriteObjectsDatabase.asset";

		private readonly FavoriteObjectsEditorPresenter _presenter;
		public FavoriteObjectsEditorModel(FavoriteObjectsEditorPresenter presenter) => _presenter = presenter;

		public void OnInitialize() {
			var path = EditorPrefs.GetString(
				"Editor.FavoriteObjects.Database.Path",
				_defaultPathSettings
			);
			var container = AssetDatabase.LoadAssetAtPath<FavoriteObjectsEditorDatabase>(path);
			if (container == null && path == _defaultPathSettings) {
				container = ScriptableObject.CreateInstance<FavoriteObjectsEditorDatabase>();
				AssetDatabase.CreateAsset(container, _defaultPathSettings);
				AssetDatabase.SaveAssets();
				Selection.activeObject = container;
			} else if (container == null) {
				D.Log("[FavoriteObjectsEditorModel]", $"Не удалось открыть базу данных избранных объектов по пути: {path}");
				return;
			}

			_presenter.FavoriteItems.Value = container.All;
		}

		public void OnSetDatabase() {
			var path = EditorUtility.OpenFilePanel(
				"Generate data",
				"Assets/LogicOff/Editor/FavoriteObjects/Settings/",
				"asset"
			);
			path = path.Replace(Application.dataPath, "Assets"); // Чистим полный путь до проекта
			if (path.Length <= 0)
				return;
			var container = AssetDatabase.LoadAssetAtPath<FavoriteObjectsEditorDatabase>(path);
			EditorPrefs.SetString("Editor.FavoriteObjects.Database.Path", path);
			Selection.activeObject = container;
			_presenter.FavoriteItems.Value = container.All;
		}

		public void OnSelection(int index) => Selection.activeObject = _presenter.FavoriteItems.Value[index].Object;
	}
}