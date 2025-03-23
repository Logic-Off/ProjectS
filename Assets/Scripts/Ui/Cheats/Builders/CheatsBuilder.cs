using Ecs.Ui;
using Utopia;

namespace Ui.Cheats {
	[InstallerPanelGenerator(InstallerId.Cheats)]
	public class CheatsBuilder : APanelBuilder {
		protected override EPanelName PanelName => EPanelName.Cheats;

		private readonly CheatsPresenter _presenter;
		private readonly CheatsInteractor _interactor;

		public CheatsBuilder(CheatsPresenter presenter, CheatsInteractor interactor) {
			_presenter = presenter;
			_interactor = interactor;
		}

		protected override void BindView(UiEntity entity) {
			_presenter.PanelId.Value = entity.Id.Value;
			_presenter.ContentContainer.Value = Find("Content").Rect.Value;
			_presenter.ButtonsContainer.Value = Find("ButtonsContainer").Rect.Value;

			Find("CloseButton").SubscribeOnClickedChange(x => _presenter.OnClose.Fire());
		}

		protected override void BindInteractor() {
			_presenter.OnActivate.AddListener(_interactor.OnActivate);
			_presenter.OnClose.AddListener(_interactor.OnClose);
			_presenter.OnRefresh.AddListener(_interactor.OnRefresh);
		}

		protected override void Activate() => _presenter.OnActivate.Fire();

		protected override void OnShow() => _presenter.OnRefresh.Fire();
	}
}