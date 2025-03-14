using Ecs.Item;

namespace Ecs.Inventory {
	public interface ICellHelper {
		void SetItem(InventoryEntity cell, ItemEntity target, int quantity);
		void Add(InventoryEntity cell, int count = 1);
		void Remove(InventoryEntity cell, int count = 1);
		bool Contains(InventoryEntity cell, ItemId itemId);
		void Destroy(InventoryEntity cell);
		int Quantity(InventoryEntity cell);
		ItemId GetItemId(InventoryEntity cell);
		void Reset(InventoryEntity cell);
	}
}