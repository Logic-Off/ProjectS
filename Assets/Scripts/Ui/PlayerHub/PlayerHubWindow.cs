using Ui.CharacterInfo;
using Ui.Craft;
using Ui.Draggable;
using Ui.PlayerInventory;
using Ui.PlayerSkills;
using Ui.Shop;
using Utopia;

namespace Ui.PlayerHub {
	[InstallerGenerator(InstallerId.Ui)]
	public class PlayerHubWindow : APrebuildWindow, IOpenWindow<EPlayerHubPanel> {
		public override EWindowName Name => EWindowName.PlayerHub;

		private readonly PlayerHubPresenter _playerHubPresenter;
		public PlayerHubWindow(PlayerHubPresenter playerHubPresenter) {
			_playerHubPresenter = playerHubPresenter;
		}

		public override void AddPanelBuilders() {
			AddBuilder<PlayerHubBuilder>();
			AddBuilder<CharacterInfoBuilder>();
			AddBuilder<PlayerSkillsBuilder>();
			AddBuilder<PlayerInventoryBuilder>();
			AddBuilder<CraftBuilder>();
			AddBuilder<ShopBuilder>();
			AddBuilder<DraggableBuilder>();
		}

		public void Open(EPlayerHubPanel panelName) {
			_playerHubPresenter.ShowPanel.Fire(panelName);
		}
	}
}