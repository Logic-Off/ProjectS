using System;
using LogicOff.Databases;
using ObjectFinderEditor.Scripts.Databases;
using UnityEditor;
using UnityEngine.UIElements;

namespace ObjectFinderEditor.Scripts {
	public sealed class ObjectFinderEditorDataBuilder : ICustomEditor {
		private readonly ObjectFinderEditorView _view;
		public string Name => "ObjectFinder";
		public void AttachTo(VisualElement parent) => parent.Add(_view);

		public void Detach() => _view.parent.Remove(_view);

		public ObjectFinderEditorDataBuilder() {
			var target = AssetDatabase.LoadAssetAtPath<ObjectFinderScriptableObject>("Assets/Editor/LogicOff/ObjectFinderEditor/ObjectFinder.asset");
			if (target == null)
				throw new NullReferenceException(
					"База данных не найдена, вы должны создать ассет по пути: Assets/Editor/LogicOff/ObjectFinderEditor/ObjectFinder.asset"
				);
			_view = new ObjectFinderEditorView(target);
			var presenter = new ObjectFinderEditorPresenter();
			var model = new ObjectFinderEditorModel(presenter);

			BindViews(_view, presenter);
			BindModels(model, presenter);

			presenter.OnData(target);
		}

		private void BindViews(ObjectFinderEditorView view, ObjectFinderEditorPresenter presenter) {
			view.SortToggle.RegisterValueChangedCallback(x => presenter.Sort.Value = x.newValue);
			view.WithAssetGroupsToggle.RegisterValueChangedCallback(x => presenter.WithAssetGroups.Value = x.newValue);
			view.DependenciesModeToggle.RegisterValueChangedCallback(x => presenter.DependenciesMode.Value = x.newValue);
			view.FindButton.clicked += presenter.OnFind.Fire;
			view.ClearButton.clicked += presenter.OnClear.Fire;
			view.RefreshPaths.clicked += presenter.OnRefreshPaths.Fire;
		}

		private void BindModels(ObjectFinderEditorModel model, ObjectFinderEditorPresenter presenter) {
			presenter.Sort.AddListener(model.OnSort);
			presenter.WithAssetGroups.AddListener(model.OnWithAssetGroups);
			presenter.DependenciesMode.AddListener(model.OnDependenciesMode);
			presenter.OnFind.AddListener(model.OnFindObjects);
			presenter.OnClear.AddListener(model.OnClear);
			presenter.OnRefreshPaths.AddListener(model.OnRefreshPaths);
		}
	}
}