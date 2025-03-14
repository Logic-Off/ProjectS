using Ecs.Common;

namespace Ecs.Inventory {
	public interface IContainerFactory {
		InventoryEntity Create(Id owner, EContainerType type, int size);
		InventoryEntity Create(Id owner, EContainerType type, string cellSettingsId);
	}
}