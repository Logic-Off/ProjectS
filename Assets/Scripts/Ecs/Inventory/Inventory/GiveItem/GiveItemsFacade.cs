using System.Collections.Generic;
using Utopia;

namespace Ecs.Inventory {
	[InstallerGenerator(InstallerId.Inventory, 3)]
	public class GiveItemsFacade : IGiveItemsFacade, IGiveItemsToStorageFacade {
		private readonly IStorage _storage;

		private readonly List<IGiveItemsListener> _listeners = new();
		private readonly List<IGiveItemsToStorageListener> _storageListeners = new();
		public GiveItemsFacade(IStorage storage) => _storage = storage;

		public void AddListener(IGiveItemsListener listener) {
			_listeners.Add(listener);
			// _listeners.Sort((a, b) => a.Order.CompareTo(b.Order));
		}

		public void RemoveListener(IGiveItemsListener listener) => _listeners.Remove(listener);
		
		public void AddListener(IGiveItemsToStorageListener listener) => _storageListeners.Add(listener);
		public void RemoveListener(IGiveItemsToStorageListener listener) => _storageListeners.Remove(listener);

		public void OnGive(List<IItemData> items) {
			if (OnGiveInventory(items))
				return;

			D.Error("[GiveItemsFacade]", "Add to storage");
			AddToStorage(items);
		}

		private void AddToStorage(List<IItemData> items) {
			foreach (var itemRecord in items)
				_storage.Add(itemRecord);

			foreach (var storageListener in _storageListeners)
				storageListener.OnGive(items);
		}

		private bool OnGiveInventory(List<IItemData> items) {
			foreach (var listener in _listeners)
				if (listener.OnGive(items))
					return true;
			return false;
		}
	}
}