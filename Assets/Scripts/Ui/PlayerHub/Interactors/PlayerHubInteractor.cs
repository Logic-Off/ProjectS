using Ui.CharacterInfo;
using Ui.Craft;
using Ui.PlayerInventory;
using Ui.PlayerSkills;
using Ui.Shop;
using Utopia;

namespace Ui.PlayerHub {
	[InstallerGenerator(InstallerId.Ui)]
	public class PlayerHubInteractor {
		private readonly CharacterInfoPresenter _characterInfoPresenter;
		private readonly PlayerSkillsPresenter _playerSkillsPresenter;
		private readonly PlayerInventoryPresenter _playerInventoryPresenter;
		private readonly CraftPresenter _craftPresenter;
		private readonly ShopPresenter _shopPresenter;
		private readonly IWindowRouter _windowRouter;

		public PlayerHubInteractor(
			CharacterInfoPresenter characterInfoPresenter,
			PlayerSkillsPresenter playerSkillsPresenter,
			PlayerInventoryPresenter playerInventoryPresenter,
			CraftPresenter craftPresenter,
			ShopPresenter shopPresenter,
			IWindowRouter windowRouter
		) {
			_characterInfoPresenter = characterInfoPresenter;
			_playerSkillsPresenter = playerSkillsPresenter;
			_playerInventoryPresenter = playerInventoryPresenter;
			_craftPresenter = craftPresenter;
			_shopPresenter = shopPresenter;
			_windowRouter = windowRouter;
		}

		public void OnShowPanel(EPlayerHubPanel panelName) {
			_characterInfoPresenter.IsVisible.Value = panelName == EPlayerHubPanel.CharacterInfo;
			_playerSkillsPresenter.IsVisible.Value = panelName == EPlayerHubPanel.PlayerSkills;
			_playerInventoryPresenter.IsVisible.Value = panelName == EPlayerHubPanel.Inventory;
			_craftPresenter.IsVisible.Value = panelName == EPlayerHubPanel.Craft;
			_shopPresenter.IsVisible.Value = panelName == EPlayerHubPanel.Shop;
		}

		public void OnClose() => _windowRouter.OnRoot();
	}
}