using System.IO;
using System.Text;
using Common;
using LogicOff.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LogicOff.CreatePanelsEditor {
	/// <summary>
	///
	/// </summary>
	[CreateAssetMenu(menuName = "Editor/Databases/CreatePanel", fileName = "CreatePanelDatabase")]
	public class CreatePanelDatabase : ScriptableObject {
		public string Path;
		public string FolderName;
		public string PanelName;
	}

	[CustomEditor(typeof(CreatePanelDatabase))]
	public sealed class CreatePanelDatabaseEditor : Editor {
		private CreatePanelDatabase _target;

		private void OnEnable() {
			_target = (CreatePanelDatabase)target;
			_target.Path = Application.dataPath + "/Scripts/Ui/";
		}

		// public override VisualElement CreateInspectorGUI() {
		// 	var rootElement = new VisualElement();
		// 	DefaultEditor.FillDefaultInspectorIMGUI(rootElement, serializedObject);
		//
		// 	var mainContainer = new VisualElement();
		// 	rootElement.Add(mainContainer);
		//
		// 	_target.Path = Application.dataPath + "/Scripts/Ui/";
		// 	D.Error("[CreatePanelDatabase]", _target.Path);
		// 	EditorUtility.SetDirty(_target);
		// 	AssetDatabase.SaveAssets();
		//
		// 	mainContainer.Add(new Button(OnCreateScripts) { text = "Create scripts" });
		// 	return rootElement;
		// }

		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			if (GUILayout.Button("Create scripts")) {
				OnCreateScripts();
				EditorUtility.SetDirty(_target);
				AssetDatabase.SaveAssets();
			}
		}

		private void OnCreateScripts() {
			if (_target.PanelName.IsNullOrEmpty()) {
				D.Error("[CreatePanelDatabase]", "Имя не может быть пустым");
				return;
			}

			var mainFolder = _target.Path + _target.FolderName;
			var buildersFolder = mainFolder + "/Builders/";
			var modelsFolder = mainFolder + "/Models/";
			var presentersFolder = mainFolder + "/Presenters/";

			CreateDirectory(mainFolder);
			CreateDirectory(buildersFolder);
			CreateDirectory(modelsFolder);
			CreateDirectory(presentersFolder);

			var builderFile = buildersFolder + _target.PanelName + "Builder.cs";
			var modelFile = modelsFolder + _target.PanelName + "Model.cs";
			var presenterFile = presentersFolder + _target.PanelName + "Presenter.cs";

			CreateFileAndWriteText(builderFile, GetBuilder());
			CreateFileAndWriteText(modelFile, GetModel());
			CreateFileAndWriteText(presenterFile, GetPresenter());
			D.Error("[CreatePanelDatabase]", _target.PanelName, "Файлы успешно созданы");
		}

		private void CreateFileAndWriteText(string path, string text) {
			if (File.Exists(path))
				File.Delete(path);

			using (FileStream stream = File.Open(path, FileMode.OpenOrCreate)) {
				byte[] info = new UTF8Encoding(true).GetBytes(text);
				stream.Write(info, 0, info.Length);
			}
		}

		private void CreateDirectory(string path) {
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
		}

		private string GetBuilder() {
			var builder = new StringBuilder();
			builder.Append("using Ecs.Ui;\n");
			builder.Append("using LogicOff;\n");
			builder.Append("using LogicOff.Windows;\n\n");
			builder.Append($"namespace Ui.{_target.FolderName}").Append(" {\n");
			builder.Append("	/// <summary>\n");
			builder.Append("	///\n");
			builder.Append("	/// </summary>\n");
			builder.Append("	[InstallUiPrefab(InstallerId.Game)]\n");
			builder.Append($"	public class {_target.PanelName}Builder : APanelBuilder ").Append("{\n");
			builder.Append($"		protected override EPanelName PanelName => EPanelName.{_target.PanelName};\n");
			builder.Append($"		private readonly {_target.PanelName}Presenter _presenter;\n");
			builder.Append($"		private readonly {_target.PanelName}Model _model;\n\n");
			builder.Append($"		public {_target.PanelName}Builder({_target.PanelName}Presenter presenter, {_target.PanelName}Model model)").Append(" {\n");
			builder.Append("			_presenter = presenter;\n");
			builder.Append("			_model = model;\n");
			builder.Append("		}\n\n");
			builder.Append("		protected override void BindViews(UiContext context, UiEntity entity) {\n");
			builder.Append("			var name = entity.Name.Value;\n			UiEntity Find(string targetName) => context.GetEntityWithName($\"{name}.{targetName}\");\n		}\n\n");
			builder.Append("		protected override void BindModels() {\n");
			builder.Append(" 			base.BindModels();\n		}\n\n		protected override void Activate() {\n			base.Activate();\n		}\n	}\n}\n");
			return builder.ToString();
		}

		private string GetModel() {
			var builder = new StringBuilder();
			builder.Append("using LogicOff;\n");
			builder.Append("using Utopia;\n\n");
			builder.Append($"namespace Ui.{_target.FolderName}").Append(" {\n");
			builder.Append("	/// <summary>\n");
			builder.Append("	///\n");
			builder.Append("	/// </summary>\n");
			builder.Append("	[InstallerGenerator(\"Game\")]\n");
			builder.Append($"	public class {_target.PanelName}Model").Append(" { }\n");
			builder.Append("}\n");
			return builder.ToString();
		}

		private string GetPresenter() {
			var builder = new StringBuilder();
			builder.Append("using LogicOff;\n");
			builder.Append("using Utopia;\n\n");
			builder.Append($"namespace Ui.{_target.FolderName}").Append(" {\n");
			builder.Append("	/// <summary>\n");
			builder.Append("	///\n");
			builder.Append("	/// </summary>\n");
			builder.Append("	[InstallerGenerator(\"Game\")]\n");
			builder.Append($"	public class {_target.PanelName}Presenter").Append(" { }\n");
			builder.Append("}\n");
			return builder.ToString();
		}
	}
}