using LogicOff.FavoriteObjects.Models;
using LogicOff.FavoriteObjects.Presenters;
using LogicOff.FavoriteObjects.Views;
using UnityEngine.UIElements;

namespace LogicOff.FavoriteObjects.Builders {
	/// <summary>
	///   Author: Andrey Abramkin
	/// </summary>
	public sealed class FavoriteObjectsEditorBuilder {
		public FavoriteObjectsEditorBuilder(VisualElement parent) {
			var presenter = new FavoriteObjectsEditorPresenter();
			var view = new FavoriteObjectsEditorView(parent);
			var model = new FavoriteObjectsEditorModel(presenter);

			BindViews(view, presenter);
			BindModels(model, presenter);

			presenter.Initialize.Fire();
		}

		private void BindViews(FavoriteObjectsEditorView view, FavoriteObjectsEditorPresenter presenter) {
			presenter.FavoriteItems.AddListener(view.SetFavoriteItems);
			view.OpenDatabaseButton.clicked += presenter.OnSetDatabase.Fire;
			view.MainList.onSelectionChange += x => presenter.OnSelection.Value = view.MainList.selectedIndex;
		}

		private void BindModels(FavoriteObjectsEditorModel model, FavoriteObjectsEditorPresenter presenter) {
			presenter.Initialize.AddListener(model.OnInitialize);
			presenter.OnSetDatabase.AddListener(model.OnSetDatabase);
			presenter.OnSelection.AddListener(model.OnSelection);
		}
	}
}