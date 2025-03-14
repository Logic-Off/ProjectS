using Ecs.Item;

namespace Ecs.Inventory {
	public interface IInventoryChangeListener {
		void OnChange(ItemId itemId, int quantity, EInventoryChangeType type);
	}
}