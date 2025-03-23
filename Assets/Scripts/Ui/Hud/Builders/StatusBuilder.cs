using Utopia;

namespace Ui.Hud {
	[InstallerPanelGenerator(InstallerId.Ui)]
	public class StatusBuilder : APanelBuilder {
		protected override EPanelName PanelName => EPanelName.Status;
		private readonly StatusPresenter _presenter;
		private readonly StatusInteractor _interactor;

		public StatusBuilder(StatusPresenter presenter, StatusInteractor interactor) {
			_presenter = presenter;
			_interactor = interactor;
		}

		protected override void BindView(UiEntity entity) {
			_presenter.HealthAmount.AddListener(x => Find("Health").ReplaceFloat(x));
			_presenter.HungerAmount.AddListener(x => Find("Hunger").ReplaceFloat(x));
			_presenter.ThirstAmount.AddListener(x => Find("Thirst").ReplaceFloat(x));
			_presenter.PsycheAmount.AddListener(x => Find("Psyche").ReplaceFloat(x));
			_presenter.ColdAmount.AddListener(x => Find("Cold").ReplaceFloat(x));
			_presenter.RadiationAmount.AddListener(x => Find("Radiation").ReplaceFloat(x));
		}

		protected override void BindInteractor() {
 			base.BindInteractor();
      _presenter.OnActivate.AddListener(_interactor.Initialize);
		}

		protected override void Activate() => _presenter.OnActivate.Fire();
	}
}
