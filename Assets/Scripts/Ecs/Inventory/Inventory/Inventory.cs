using System.Collections.Generic;
using Common;
using Ecs.Item;
using Ecs.Save;
using Ecs.Shared;
using Utopia;

namespace Ecs.Inventory {
	[InstallerGenerator(InstallerId.Inventory, 100_000, EInstallType.NonLazy, EBindType.BindInterfacesTo)]
	public class Inventory : IInventory {
		private readonly SharedContext _shared;
		private readonly IInventoryChangeFacade _inventoryChangeFacade;
		private readonly IItemsDatabase _itemsDatabase;

		public Inventory(SharedGlobalContext shared, IDataService dataService, IInventoryChangeFacade inventoryChangeFacade, IItemsDatabase itemsDatabase) {
			_shared = shared;
			_inventoryChangeFacade = inventoryChangeFacade;
			_itemsDatabase = itemsDatabase;
			var inventory = dataService.GetObject(InventorySaveProcessor.SaveKey, new Dictionary<ItemId, int>());
			_shared.SetInventory(inventory);
		}

		public void Add(ItemId itemId, int quantity) {
#if DEBUG
			D.Warning("[Inventory.Add]", itemId, quantity);
#endif
			var inventory = _shared.Inventory.Values;
			if (!inventory.ContainsKey(itemId))
				inventory.Add(itemId, quantity.Min(GetStackSize(itemId)));
			else
				inventory[itemId] = (inventory[itemId] + quantity).Min(GetStackSize(itemId));
			
			_shared.InventoryEntity.ReplaceInventory(inventory);

			_inventoryChangeFacade.OnChange(itemId, quantity, EInventoryChangeType.Added);
		}

		public void Remove(ItemId itemId, int quantity) {
#if DEBUG
			D.Warning("[Inventory.Remove]", itemId, quantity);
#endif
			var inventory = _shared.Inventory.Values;
			if (!inventory.ContainsKey(itemId))
				return;
			inventory[itemId] -= quantity;
			_shared.InventoryEntity.ReplaceInventory(inventory);
			_inventoryChangeFacade.OnChange(itemId, quantity, EInventoryChangeType.Removed);
		}

		public int Count(ItemId itemId) {
			var inventory = _shared.Inventory.Values;
			var hasKey = inventory.ContainsKey(itemId);
			return hasKey ? inventory[itemId] : 0;
		}

		public bool Has(ItemId itemId) {
			var inventory = _shared.Inventory.Values;
			if (!inventory.ContainsKey(itemId))
				return false;

			return inventory[itemId] > 0;
		}

		private int GetStackSize(ItemId itemId) => 
			_itemsDatabase.Has(itemId) ? _itemsDatabase.Get(itemId).StackSize : int.MaxValue;
	}
}