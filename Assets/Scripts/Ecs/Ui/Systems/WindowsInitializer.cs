using System.Collections.Generic;
using Ecs.Common;
using Ui;
using Utopia;

namespace Ecs.Ui {
	[InstallerGenerator(InstallerId.Ui, 1000)]
	public class WindowsInitializer : IOnSceneLoadedListener {
		private readonly List<IWindow> _windows;
		private readonly IWindowRouter _router;

		public WindowsInitializer(List<IWindow> windows, IWindowRouter router) {
			_windows = windows;
			_router = router;
		}

		public void OnSceneLoaded() {
			foreach (var window in _windows)
				_router.AddWindow(window);
		}
	}
}