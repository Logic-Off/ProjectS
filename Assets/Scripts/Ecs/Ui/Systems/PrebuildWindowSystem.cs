using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Ecs.Common;
using Ui;
using Utopia;

namespace Ecs.Ui {
	[InstallerGenerator(InstallerId.Ui, 5000)]
	public class PrebuildWindowSystem : IOnSceneLoadedListener {
		private readonly IWindowRouter _router;
		private readonly List<IPrebuildWindow> _windows;

		public PrebuildWindowSystem(
			IWindowRouter router,
			List<IPrebuildWindow> windows
		) {
			_router = router;
			_windows = windows;
		}

		public void OnSceneLoaded() => Prebuild();

		private async UniTaskVoid Prebuild() {
			var tasks = new List<UniTask>();
			foreach (var window in _windows) {
				_router.GetWindow(window.Name);
				tasks.Add(window.Prebuild());
			}

			await UniTask.WhenAll(tasks);
		}
	}
}