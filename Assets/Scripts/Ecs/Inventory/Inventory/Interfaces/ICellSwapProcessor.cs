namespace Ecs.Inventory {
	public interface ICellSwapProcessor {
		void Swap(CellId departureId, CellId destinationId);
		void Swap(InventoryEntity departure, InventoryEntity destination);
	}
}