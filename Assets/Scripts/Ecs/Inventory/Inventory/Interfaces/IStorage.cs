using System.Collections.Generic;
using Ecs.Item;

namespace Ecs.Inventory {
	public interface IStorage {
		List<StorageEntry> Items { get; }
		void Add(IItemData data);
		void Remove(IItemData data);
		bool Has(ItemId itemId);
	}
}