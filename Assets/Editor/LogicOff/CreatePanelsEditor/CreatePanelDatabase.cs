using System.IO;
using System.Text;
using Common;
using UnityEditor;
using UnityEngine;

namespace LogicOff.CreatePanelsEditor {
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
			var interactorsFolder = mainFolder + "/Interactors/";
			var presentersFolder = mainFolder + "/Presenters/";

			CreateDirectory(mainFolder);
			CreateDirectory(buildersFolder);
			CreateDirectory(interactorsFolder);
			CreateDirectory(presentersFolder);

			var builderFile = buildersFolder + _target.PanelName + "Builder.cs";
			var interactorFile = interactorsFolder + _target.PanelName + "Interactor.cs";
			var presenterFile = presentersFolder + _target.PanelName + "Presenter.cs";

			CreateFileAndWriteText(builderFile, GetBuilder());
			CreateFileAndWriteText(interactorFile, GetInteractor());
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
			builder.AppendLine("using Utopia;\n");
			builder.Append($"namespace Ui.{_target.FolderName}").AppendLine(" {");
			builder.AppendLine("	[InstallerPanelGenerator(InstallerId.Ui)]");
			builder.Append($"	public class {_target.PanelName}Builder : APanelBuilder ").AppendLine("{");
			builder.AppendLine($"		protected override EPanelName PanelName => EPanelName.{_target.PanelName};");
			builder.AppendLine($"		private readonly {_target.PanelName}Presenter _presenter;");
			builder.AppendLine($"		private readonly {_target.PanelName}Interactor _interactor;\n");
			builder.Append($"		public {_target.PanelName}Builder({_target.PanelName}Presenter presenter, {_target.PanelName}Interactor interactor)").AppendLine(" {");
			builder.AppendLine("			_presenter = presenter;");
			builder.AppendLine("			_interactor = interactor;");
			builder.AppendLine("		}\n");
			builder.AppendLine("		protected override void BindView(UiContext context, UiEntity entity) {");
			builder.AppendLine("		}\n");
			builder.AppendLine("		protected override void BindInteractor() {");
			builder.AppendLine(" 			base.BindInteractor();");
			builder.AppendLine("		}\n");
			builder.AppendLine("		protected override void Activate() {");
			builder.AppendLine("			base.Activate();");
			builder.AppendLine("		}");
			builder.AppendLine("	}");
			builder.AppendLine("}");
			return builder.ToString();
		}

		private string GetInteractor() {
			var builder = new StringBuilder();
			builder.AppendLine("using Utopia;\n");
			builder.Append($"namespace Ui.{_target.FolderName}").AppendLine(" {");
			builder.AppendLine("	[InstallerGenerator(InstallerId.Ui)]");
			builder.Append($"	public class {_target.PanelName}Interactor").AppendLine(" { }");
			builder.AppendLine("}");
			return builder.ToString();
		}

		private string GetPresenter() {
			var builder = new StringBuilder();
			builder.AppendLine("using Utopia;\n");
			builder.Append($"namespace Ui.{_target.FolderName}").AppendLine(" {");
			builder.AppendLine("	[InstallerGenerator(InstallerId.Ui)]");
			builder.Append($"	public class {_target.PanelName}Presenter").AppendLine(" { }");
			builder.AppendLine("}");
			return builder.ToString();
		}
	}
}