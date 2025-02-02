using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace LogicOff.Databases {
	/// <summary>
	/// </summary>
	public sealed class DatabaseEditorBuilder {
		private DatabaseEditorPresenter _presenter;
		private DatabasesEditorView _view;

		public DatabaseEditorBuilder(VisualElement parent) => Initialize(parent);

		private async void Initialize(VisualElement parent) {
			_presenter = new DatabaseEditorPresenter();
			_view = new DatabasesEditorView(parent);
			var model = new DatabasesEditorModel(_presenter);

			await Task.Delay(2000);
			BindViews(_view, _presenter);
			BindModels(model, _presenter);

			_presenter.InfoContainer.Value = _view.InfoContainer;
			_presenter.OnRedraw.Fire();
			SetSelection(_presenter, _view);
		}

		private void SetSelection(DatabaseEditorPresenter presenter, DatabasesEditorView view) {
			if (presenter.SelectedPrimeIndex.Value >= 0)
				view.PrimeList.SetSelection(presenter.SelectedPrimeIndex.Value);
		}

		private void BindViews(DatabasesEditorView view, DatabaseEditorPresenter presenter) {
			view.PrimeList.onSelectionChange +=
				_ => {
					presenter.SelectedPrimeIndex.Value = view.PrimeList.selectedIndex;
					presenter.SelectedPrimeObject.Value = view.PrimeList.selectedItem;
				};
			view.SubList.onSelectionChange += _ => {
				presenter.SelectedSubIndex.Value = view.SubList.selectedIndex;
				presenter.SelectedSubObject.Value = view.SubList.selectedItem;
			};

			presenter.PrimeListElements.AddListener(view.SetPrimeListElements);

			presenter.CurrentDatabase.AddListener(view.SetDatabase);
			presenter.Databases.AddListener(view.SetSubListElements);
		}

		private void BindModels(DatabasesEditorModel model, DatabaseEditorPresenter presenter) {
			presenter.OnRedraw.AddListener(model.OnInitialize);

			presenter.SelectedPrimeIndex.AddListener(model.OnSelectedPrimeIndex);
			presenter.SelectedSubIndex.AddListener(model.OnSelectedSubIndex);
			presenter.SelectedPrimeObject.AddListener(model.OnSelectedPrimeObject);
			presenter.SelectedSubObject.AddListener(model.OnSelectDatabase);
		}

		public void Redraw() {
			_presenter.OnRedraw.Fire();
			SetSelection(_presenter, _view);
		}
	}
}