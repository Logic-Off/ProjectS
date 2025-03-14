using System.Collections.Generic;
using Ecs.Item;

namespace Ecs.Inventory {
	public interface IStackManipulator {
		bool Put(List<InventoryEntity> cells, ItemId itemId, int quantity);
		(bool allPut, int remainder) PutAny(List<InventoryEntity> cells, ItemId itemId, int quantity);
		void Remove(List<InventoryEntity> cells, ItemId itemId, int amount);
		void RemoveAll(List<InventoryEntity> cells, ItemId itemId);
		bool IsEnoughSpace(List<InventoryEntity> cells, ItemId itemId, int amount);
		bool IsEnoughSpace(List<InventoryEntity> cells, IReadOnlyList<IItemData> items);
		int Count(List<InventoryEntity> cells, ItemId itemId);
		bool Contains(List<InventoryEntity> cells, ItemId itemId);
		(bool result, ItemId itemId) ContainsAnyOf(List<InventoryEntity> cells, IEnumerable<ItemId> items);
		(bool result, ItemInstanceId instanceId) FindAnyInstanceOf(List<InventoryEntity> cells, ItemId item);
		(bool result, ItemInstanceId instanceId) FindAnyInstanceOf(List<InventoryEntity> cells, IEnumerable<ItemId> items);
		bool FindAllInstancesOf(List<InventoryEntity> cells, IEnumerable<ItemId> items, List<ItemInstanceId> buffer);
		void PackedItems(List<InventoryEntity> cells, List<(ItemId, int)> buffer, bool sortSmallToLarge = true);
		int GetAvailableSpace(List<InventoryEntity> cells, ItemId itemId);
		bool Split(List<InventoryEntity> cells, InventoryEntity targetCell);
	}
}