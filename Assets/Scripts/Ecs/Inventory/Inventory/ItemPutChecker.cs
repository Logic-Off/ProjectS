using Ecs.Item;
using Ecs.Inventory;
using Utopia;

namespace Ecs.Inventory {
	/// <summary>
	/// Класс проверяет можно ли переместить в контейнер итем 
	/// </summary>
	[InstallerGenerator(InstallerId.Game)]
	public class ItemPutChecker {
		private readonly InventoryContext _inventory;
		private readonly ItemContext _items;

		public ItemPutChecker(
			InventoryContext inventory,
			ItemContext items
		) {
			_inventory = inventory;
			_items = items;
		}
		
		public bool CanSwapCells(InventoryEntity departure, InventoryEntity destination) {
			// Если один и тот же контейнер то можем переложить
			if (departure.ContainerOwner.Value == destination.ContainerOwner.Value)
				return true;

			var departureContainer = _inventory.GetEntityWithContainerId(departure.ContainerOwner.Value);
			var destinationContainer = _inventory.GetEntityWithContainerId(destination.ContainerOwner.Value);

			var checkResult = CheckByCell(departure, destinationContainer) && CheckByCell(destination, departureContainer);
			return checkResult;
		}

		/// <summary>
		/// Проверяет можно ли поиместить итем из конкретной ячейки в контейнер, если ячейка пустая то возвращает true
		/// </summary>
		public bool CheckByCell(InventoryEntity cell, InventoryEntity container) {
			if (cell.IsEmpty)
				return true;

			var item = _items.GetEntityWithCellId(cell.CellId.Value);
			return CheckByItem(item, container);
		}

		/// <summary>
		/// Проверяет можно ли поместить существующий итем в контейнер
		/// </summary>
		public bool CheckByItem(ItemEntity item, InventoryEntity container)
			=> CheckByType(item.ItemType.Value, container);

		/// <summary>
		/// Проверка по типу, можно ли поместить в контейнер конкретный тип предмета
		/// </summary>
		public bool CheckByType(EItemType type, InventoryEntity container)
			=> container.ItemTypes.Value.Contains(type);
	}
}