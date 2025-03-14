namespace Ecs.Inventory {
	public interface IGiveItemsToStorageFacade {
		void AddListener(IGiveItemsToStorageListener listener);
		void RemoveListener(IGiveItemsToStorageListener listener);
	}
}