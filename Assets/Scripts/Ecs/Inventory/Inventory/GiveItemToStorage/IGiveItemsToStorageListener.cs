using System.Collections.Generic;
using Ecs.Item;

namespace Ecs.Inventory {
	public interface IGiveItemsToStorageListener {
		void OnGive(List<IItemData> items);
	}
}