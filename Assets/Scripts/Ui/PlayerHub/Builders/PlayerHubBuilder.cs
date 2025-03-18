using Ecs.Ui;
using Utopia;

namespace Ui.PlayerHub {
	[InstallerPanelGenerator(InstallerId.Ui)]
	public class PlayerHubBuilder : APanelBuilder {
		protected override EPanelName PanelName => EPanelName.PlayerHub;
		private readonly PlayerHubPresenter _presenter;
		private readonly PlayerHubInteractor _interactor;

		public PlayerHubBuilder(PlayerHubPresenter presenter, PlayerHubInteractor interactor) {
			_presenter = presenter;
			_interactor = interactor;
		}

		protected override void BindView(UiContext context, UiEntity entity) {
			Find("CharacterInfoButton").SubscribeOnClickedChange(x => _presenter.ShowPanel.Fire(EPlayerHubPanel.CharacterInfo));
			Find("PlayerSkillsButton").SubscribeOnClickedChange(x => _presenter.ShowPanel.Fire(EPlayerHubPanel.PlayerSkills));
			Find("InventoryButton").SubscribeOnClickedChange(x => _presenter.ShowPanel.Fire(EPlayerHubPanel.Inventory));
			Find("CraftButton").SubscribeOnClickedChange(x => _presenter.ShowPanel.Fire(EPlayerHubPanel.Craft));
			Find("ShopButton").SubscribeOnClickedChange(x => _presenter.ShowPanel.Fire(EPlayerHubPanel.Shop));
			Find("CloseButton").SubscribeOnClickedChange(x => _presenter.OnClose.Fire());
		}

		protected override void BindInteractor() {
			base.BindInteractor();
			_presenter.ShowPanel.AddListener(_interactor.OnShowPanel);
			_presenter.OnClose.AddListener(_interactor.OnClose);
		}

		protected override void Activate() {
			base.Activate();
		}
	}
}