using Ecs.Inventory;

namespace Ecs.Item {
	public interface IItemsFactory {
		ItemEntity Create(ItemId itemId);
		void Destroy(ItemInstanceId id);
	}
}