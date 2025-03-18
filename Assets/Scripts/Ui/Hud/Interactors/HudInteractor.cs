using Ui.PlayerHub;
using Utopia;

namespace Ui.Hud {
	[InstallerGenerator(InstallerId.Ui)]
	public class HudInteractor {
		private readonly IWindowRouter _windowRouter;
		public HudInteractor(IWindowRouter windowRouter) => _windowRouter = windowRouter;

		public void OpenInventory() => _windowRouter.OpenWindow(EWindowName.PlayerHub, EPlayerHubPanel.Inventory);
	}
}
