using ObjectFinderEditor.Scripts.Databases;
using UnityEditor;
using UnityEngine.UIElements;

namespace ObjectFinderEditor.Scripts {
	public sealed class ObjectFinderEditorView : VisualElement {
		public readonly Toggle SortToggle;
		public readonly Toggle WithAssetGroupsToggle;
		public readonly Toggle DependenciesModeToggle;
		public readonly Button FindButton;
		public readonly Button ClearButton;
		public readonly Button RefreshPaths;

		private readonly ObjectFinderScriptableObject _target;
		private Editor _editor;
		private readonly ScrollView _scrollView;

		public ObjectFinderEditorView(ObjectFinderScriptableObject target) {
			_target = target;
			styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/LogicOff/styles.uss"));
			styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/LogicOff/ObjectFinderEditor/styles.uss"));

			AddToClassList("objectFinder-panel");

			_scrollView = new ScrollView();

			var container = new VisualElement();
			container.AddToClassList("objectFinder-buttonContainer");
			var container1 = new VisualElement();
			container1.AddToClassList("objectFinder-rightContainer");

			var rightTogglesContainer = new VisualElement();
			rightTogglesContainer.AddToClassList("objectFinder-rightTogglesContainer");

			SortToggle = new Toggle("Sort");
			SortToggle.AddToClassList("objectFinder-toggle");

			WithAssetGroupsToggle = new Toggle("Asset groups");
			WithAssetGroupsToggle.AddToClassList("objectFinder-toggle");

			DependenciesModeToggle = new Toggle("DependenciesMode");
			DependenciesModeToggle.AddToClassList("objectFinder-toggle");

			FindButton = new Button { name = "Find objects button", text = "Find objects" };
			FindButton.AddToClassList("objectFinder-findObjectsButton");

			ClearButton = new Button { name = "Clear button", text = "Clear" };
			ClearButton.AddToClassList("objectFinder-findObjectsButton");

			RefreshPaths = new Button { name = "Refresh paths", text = @"Обновить пути (Если были созданы\удалены\перемещены файлы)" };
			RefreshPaths.AddToClassList("objectFinder-refreshPathsButton");

			container.Add(FindButton);
			container.Add(ClearButton);
			container.Add(container1);
			container1.Add(rightTogglesContainer);
			rightTogglesContainer.Add(SortToggle);
			rightTogglesContainer.Add(WithAssetGroupsToggle);
			rightTogglesContainer.Add(DependenciesModeToggle);
			Add(RefreshPaths);
			Add(container);
			Add(_scrollView);
			_scrollView.Add(new IMGUIContainer(OnGUI));
		}

		private void OnGUI() {
			if (_target != null)
				Editor.DrawFoldoutInspector(_target, ref _editor);
		}
	}
}