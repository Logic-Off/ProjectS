using Ecs.Item;

namespace Ecs.Inventory {
	public interface IInventory {
		void Add(ItemId itemId, int quantity);
		void Remove(ItemId itemId, int quantity);
		int Count(ItemId itemId);
		bool Has(ItemId itemId);
	}
}