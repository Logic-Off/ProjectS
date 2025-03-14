using System.Collections.Generic;
using Utopia;

namespace Ecs.Items {
	[InstallerGenerator(InstallerId.Game)]
	public class ChangeActiveItemController : IChangeActiveItemController {
		private readonly List<IChangeActiveItemListener> _listeners;

		public ChangeActiveItemController(List<IChangeActiveItemListener> listeners) => _listeners = listeners;

		public void Activate(ItemEntity item) {
			foreach (var listener in _listeners)
				listener.Activate(item);
		}

		public void Deactivate(ItemEntity item) {
			foreach (var listener in _listeners)
				listener.Deactivate(item);
		}
	}
}