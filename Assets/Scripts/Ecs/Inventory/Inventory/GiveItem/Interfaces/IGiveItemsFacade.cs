using System.Collections.Generic;
using Ecs.Item;

namespace Ecs.Inventory {
	public interface IGiveItemsFacade {
		void AddListener(IGiveItemsListener listener);
		void RemoveListener(IGiveItemsListener listener);
		void OnGive(List<IItemData> items);
	}
}