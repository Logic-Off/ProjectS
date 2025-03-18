using Utopia;

namespace Ui.Craft {
	[InstallerPanelGenerator(InstallerId.Ui)]
	public class CraftBuilder : APanelBuilder {
		protected override EPanelName PanelName => EPanelName.Craft;
		private readonly CraftPresenter _presenter;
		private readonly CraftInteractor _interactor;

		public CraftBuilder(CraftPresenter presenter, CraftInteractor interactor) {
			_presenter = presenter;
			_interactor = interactor;
		}

		protected override void BindView(UiContext context, UiEntity entity) {
			_presenter.IsVisible.AddListener(x => entity.IsVisible = x);
		}

		protected override void BindInteractor() {
 			base.BindInteractor();
		}

		protected override void Activate() {
			base.Activate();
		}
	}
}
