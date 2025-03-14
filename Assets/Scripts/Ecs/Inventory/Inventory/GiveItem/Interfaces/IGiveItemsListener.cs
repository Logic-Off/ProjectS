using System.Collections.Generic;
using Ecs.Item;

namespace Ecs.Inventory {
	public interface IGiveItemsListener {
		bool OnGive(List<IItemData> items);
	}
}