using System.Collections.Generic;
using Ecs.Item;
using Utopia;

namespace Ecs.Inventory {
	/// <summary>
	/// Фасад сообщающий всем слушателям что инвентарь изменился.
	/// </summary>
	[InstallerGenerator(InstallerId.Inventory, 100_000, EInstallType.None, EBindType.BindInterfacesTo)]
	public class InventoryChangeFacade : IInventoryChangeFacade {
		private readonly List<IInventoryChangeListener> _listeners = new();

		public void OnChange(ItemId itemId, int quantity, EInventoryChangeType type) {
			foreach (var listener in _listeners)
				listener.OnChange(itemId, quantity, type);
		}

		public void AddListener(IInventoryChangeListener listener) => _listeners.Add(listener);
		public void RemoveListener(IInventoryChangeListener listener) => _listeners.Add(listener);
	}
}