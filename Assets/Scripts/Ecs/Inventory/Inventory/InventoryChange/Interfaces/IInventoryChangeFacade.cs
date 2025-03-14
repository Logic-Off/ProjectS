using Ecs.Item;

namespace Ecs.Inventory {
	public interface IInventoryChangeFacade {
		void OnChange(ItemId itemId, int quantity, EInventoryChangeType type);
		void AddListener(IInventoryChangeListener listener);
		void RemoveListener(IInventoryChangeListener listener);
	}
}