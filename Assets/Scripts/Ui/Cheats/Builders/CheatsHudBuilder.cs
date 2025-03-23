using Ecs.Ui;
using Utopia;

namespace Ui.Cheats {
	[InstallerPanelGenerator(InstallerId.Cheats)]
	public class CheatsHudBuilder : APanelBuilder {
		protected override EPanelName PanelName => EPanelName.CheatsHud;
		private readonly CheatsHudPresenter _presenter;
		private readonly CheatsHudInteractor _interactor;

		public CheatsHudBuilder(CheatsHudPresenter presenter, CheatsHudInteractor interactor) {
			_presenter = presenter;
			_interactor = interactor;
		}

		protected override void BindView(UiEntity entity) {
			Find("CheatsButton").SubscribeOnClickedChange(x => _presenter.OnOpenCheats.Fire());
		}

		protected override void BindInteractor() {
			base.BindInteractor();
			_presenter.OnOpenCheats.AddListener(_interactor.OpenCheats);
			_presenter.OnActivate.AddListener(_interactor.OnActivate);
		}

		protected override void Activate() {
			_presenter.OnActivate.Fire();
		}
	}
}