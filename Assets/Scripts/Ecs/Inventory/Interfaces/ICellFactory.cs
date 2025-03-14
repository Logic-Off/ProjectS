namespace Ecs.Inventory {
	public interface ICellFactory {
		InventoryEntity Create(ContainerId containerId);
	}
}