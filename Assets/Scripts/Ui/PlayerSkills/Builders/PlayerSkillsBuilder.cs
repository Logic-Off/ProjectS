using Utopia;

namespace Ui.PlayerSkills {
	[InstallerPanelGenerator(InstallerId.Ui)]
	public class PlayerSkillsBuilder : APanelBuilder {
		protected override EPanelName PanelName => EPanelName.PlayerSkills;
		private readonly PlayerSkillsPresenter _presenter;
		private readonly PlayerSkillsInteractor _interactor;

		public PlayerSkillsBuilder(PlayerSkillsPresenter presenter, PlayerSkillsInteractor interactor) {
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