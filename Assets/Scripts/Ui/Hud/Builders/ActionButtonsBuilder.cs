using Ecs.Ui;
using Utopia;

namespace Ui.Hud {
	[InstallerPanelGenerator(InstallerId.Ui)]
	public class ActionButtonsBuilder : APanelBuilder {
		protected override EPanelName PanelName => EPanelName.ActionButtons;
		private readonly ActionButtonsPresenter _presenter;
		private readonly ActionButtonsInteractor _interactor;

		public ActionButtonsBuilder(ActionButtonsPresenter presenter, ActionButtonsInteractor interactor) {
			_presenter = presenter;
			_interactor = interactor;
		}

		protected override void BindView(UiContext context, UiEntity entity) {
			Find("DefaultAbility").SubscribeOnClickedChange(x => _presenter.OnCastDefaultAbility.Fire());
		}

		protected override void BindInteractor() {
 			base.BindInteractor();
      _presenter.OnCastDefaultAbility.AddListener(_interactor.CastDefaultAbility);
		}

		protected override void Activate() {
			base.Activate();
		}
	}
}
