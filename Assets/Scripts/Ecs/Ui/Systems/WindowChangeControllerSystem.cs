using System.Collections.Generic;
using Ui;
using Utopia;
using Zenject;

namespace Ecs.Ui {
	[InstallerGenerator(InstallerId.Ui, 4000)]
	public class WindowChangeControllerSystem : IInitializable {
		private readonly IOnChangeWindowController _controller;
		private readonly List<IOnWindowChangeListener> _listeners;

		public WindowChangeControllerSystem(IOnChangeWindowController controller, List<IOnWindowChangeListener> listeners) {
			_controller = controller;
			_listeners = listeners;
		}

		public void Initialize() => _controller.SubscribeWindowChange(OnWindowChange);

		private void OnWindowChange(IWindow window) {
			foreach (var listener in _listeners)
				listener.OnWindowChange(window);
		}
	}
}