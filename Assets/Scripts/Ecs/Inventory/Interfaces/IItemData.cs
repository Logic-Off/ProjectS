using Ecs.Item;

namespace Ecs.Inventory {
	public interface IItemData {
		ItemId Id { get; }
		int Quantity { get; }
	}
}