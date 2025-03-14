using System.Collections.Generic;
using Ecs.Item;

namespace Ecs.Inventory {
	public interface ICellManipulator {
		bool Put(InventoryEntity cell, ItemId itemId, int quantity);
		(bool allPut, int remainder) PutAny(InventoryEntity cell, ItemId itemId, int quantity);
		void Remove(InventoryEntity entity, ItemId itemId, ref int amount);
		void RemoveAll(InventoryEntity entity, ItemId itemId);
		bool IsEnoughSpace(InventoryEntity cells, ItemId itemId, int amount);
		bool IsEnoughSpace(InventoryEntity entity, IReadOnlyList<(ItemId itemId, int amount)> items);
		int Count(InventoryEntity entity, ItemId itemId);
		bool Contains(InventoryEntity entity, ItemId itemId);
		(bool, ItemId) ContainsAnyOf(InventoryEntity cell, IEnumerable<ItemId> items);
		(bool, ItemInstanceId) FindAnyInstanceOf(InventoryEntity cell, ItemId item);
		(bool, ItemInstanceId) FindAnyInstanceOf(InventoryEntity cell, IEnumerable<ItemId> items);
		bool FindAllInstancesOf(InventoryEntity cell, IEnumerable<ItemId> items, List<ItemInstanceId> buffer);
		void PackedItems(InventoryEntity entity, List<(ItemId, int)> buffer, bool sortSmallToLarge = true);
		int GetAvailableSpace(InventoryEntity entity, ItemId itemId);
		void Destroy(InventoryEntity cell);
	}
}