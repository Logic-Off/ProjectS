using Utopia;

namespace Ui.Cheats {
	[InstallerGenerator(InstallerId.Cheats)]
	public class CheatsHudInteractor {
		private readonly IWindowRouter _windowRouter;

		public CheatsHudInteractor(IWindowRouter windowRouter) {
			_windowRouter = windowRouter;
		}

		public void OpenCheats() => _windowRouter.OpenWindow(EWindowName.Cheats);

		public void OnActivate() { }
	}
}