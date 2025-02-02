using System;
using System.IO;
using Common;
using Installers;
using JCMG.Genesis.Editor;
using LogicOff.Databases;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace LogicOff.Toolbar {
	/// <summary>
	///   Расширения для редактора
	///   Author: Andrey Abramkin
	/// </summary>
	public static class Extensions {
		public static void LoadStyles(this VisualElementStyleSheetSet styleSheets, string path)
			=> styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(path));

		public static TemplateContainer LoadUxml(this VisualElement self, string name) {
			var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(name);
			var template = asset.Instantiate();
			self.Add(template);
			return template;
		}

		public static TemplateContainer LoadUxml(string name) {
			var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(name);
			var template = asset.Instantiate();
			return template;
		}

		public static void OnGenerate(string option) {
			switch (option) {
				case "Installers":
					InstallerGeneratorExtensions.Generate();
					break;
				case "ECS":
					GenesisCLIRunner.RunCodeGeneration();
					break;
				default:
					throw new Exception($"Option {option} not found");
			}
		}

		public static void OnOpenIDE() => EditorApplication.ExecuteMenuItem("Assets/Open C# Project");

		public static void OnOpenDatabases() => DatabasesEditorWindow.Open();

		public static void OnDeleteSave() {
			File.Delete(Application.persistentDataPath + "/Save.save");
			D.Log("Save was deleted");
		}

		public static void OnLoadScene(string scene) {
			var s = EditorSceneManager.OpenScene($"Assets/Scenes/{scene}/{scene}.unity", OpenSceneMode.Single);
			var asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(s.path);
			Selection.activeObject = asset;
		}

		public static void OnSetFirstScene(string scene) {
			EditorPrefs.SetString("Editor.FirstScene", scene);
		}

		public static void OnSave(string option) {
			var split = option.Split('/');
			switch (split[0]) {
				case "Load":
					OnLoadSave(split);
					break;
				case "Save":
					OnCopySave(split);
					break;
				case "Delete":
					if (File.Exists(DefaultSave())) {
						File.Delete(DefaultSave());
						D.Error("[DeleteSave]", "Сохранение удалено");
					}

					break;
			}
		}

		private static void OnLoadSave(string[] split) {
			var path = GetSaveFilePath(split[1]);
			if (!File.Exists(path)) {
				D.Error("[LoadSave]", $"Сохранение {split[1]} не найдено.");
				return;
			}

			File.Copy(path, DefaultSave(), true);
			D.Error("[LoadSave]", $"Сохранение {split[1]} загружено.");
		}

		private static void OnCopySave(string[] split) {
			var path = GetSaveFilePath(split[1]);
			var defaultSave = DefaultSave();
			if (!File.Exists(defaultSave)) {
				D.Error("[CopySave]", $"Сохранение не найдено.");
				return;
			}

			File.Copy(defaultSave, path, true);
			D.Error("[CopySave]", $"Сохранение скопировано в {split[1]}.");
		}

		private static string DefaultSave() => Path.Combine(Application.persistentDataPath, "Save.save");
		private static string GetSaveFilePath(string value) => Path.Combine(Application.persistentDataPath, $"Save_{value}.save");

		public static void OnChangeAutoStartScene() {
			AutoStartScene.IsAutoSetStartScene = !AutoStartScene.IsAutoSetStartScene;
		}
	}
}