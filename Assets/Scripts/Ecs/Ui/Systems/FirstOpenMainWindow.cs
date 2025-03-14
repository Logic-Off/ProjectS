using Ecs.Common;
using Ui;
using Utopia;

namespace Ecs.Ui {
	[InstallerGenerator(InstallerId.Ui, 6000)]
	public class FirstOpenMainWindow : IOnSceneLoadedListener {
		private readonly IMainWindow _window;
		private readonly IMainWindowController _controller;

		public FirstOpenMainWindow(IMainWindow window, IMainWindowController controller) {
			_window = window;
			_controller = controller;
		}

		public void OnSceneLoaded() {
			_controller.SetMainWindow(_window.Name);
			_controller.OpenWindow(_window.Name);
		}
	}
}