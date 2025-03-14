using System.Collections.Generic;
using Ecs.Item;

namespace Ecs.Inventory {
	public interface IPlayerInventory {
		bool Add(ItemId itemId, int quantity);
		void Remove(ItemId itemId, int quantity);
		int Count(ItemId itemId);
		bool Has(ItemId itemId);
		bool IsEnoughSpace(ItemId itemId, int quantity = 1);
		bool IsEnoughSpace(List<IItemData> items);
		ItemInstanceId Find(ItemId itemId);
		void FindAll(ItemId itemId, ref List<ItemInstanceId> buffer);
		void AllItems(ref List<ItemInstanceId> items);
	}
}