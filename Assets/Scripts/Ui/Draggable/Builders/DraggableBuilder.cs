using Utopia;

namespace Ui.Draggable {
	[InstallerPanelGenerator(InstallerId.Ui)]
	public class DraggableBuilder : APanelBuilder {
		protected override EPanelName PanelName => EPanelName.Draggable;
		private readonly DraggablePresenter _presenter;
		private readonly DraggableInteractor _interactor;

		public DraggableBuilder(DraggablePresenter presenter, DraggableInteractor interactor) {
			_presenter = presenter;
			_interactor = interactor;
		}

		protected override void BindView(UiEntity entity) {
			var icon = Find($"Icon");
			_presenter.Position.AddListener(x => icon.ReplaceVector2(x));
			_presenter.IconId.AddListener(x => icon.ReplaceIconId(x));
			_presenter.IsVisible.AddListener(x => Find($"Container").IsVisible = x);
		}

		protected override void BindInteractor() {
			_presenter.OnHide.AddListener(_interactor.OnHide);
		}

		protected override void OnHide() => _presenter.OnHide.Fire();

		protected override void Activate() => _presenter.IsVisible.Fire();
	}
}