using System;
using System.Collections.Generic;
using Ecs.Item;
using Ecs.Save;
using Ecs.Shared;
using Utopia;

namespace Ecs.Inventory {
	[Serializable]
	public class StorageEntry {
		public ItemId Id;
		public int Quantity;
		public bool IsNew;

		public StorageEntry(ItemId id, int quantity, bool isNew) {
			Id = id;
			Quantity = quantity;
			IsNew = isNew;
		}
	}

	/// <summary>
	/// Хранилище игрока, хранит в себе итемы которые не могут попасть в инвентарь игрока по тем или иным причинам
	/// </summary>
	[InstallerGenerator(InstallerId.Inventory, 100_000, EInstallType.NonLazy, EBindType.BindInterfacesTo)]
	public class Storage : IStorage {
		private readonly SharedContext _shared;

		public List<StorageEntry> Items => _shared.Storage.Values;

		public Storage(SharedGlobalContext shared, IDataService dataService) {
			_shared = shared;
			var storage = dataService.GetObject(StorageSaveProcessor.SaveKey, new List<StorageEntry>());
			_shared.SetStorage(storage);
		}

		public void Add(IItemData data) {
#if DEBUG
			D.Error("[Storage.Add]", data.Id, data.Quantity);
#endif
			var storage = _shared.Storage.Values;
			storage.Add(new StorageEntry(data.Id, data.Quantity, true));
			_shared.StorageEntity.ReplaceStorage(storage);
		}

		public void Remove(IItemData data) {
#if DEBUG
			D.Error("[Storage.Remove]", data.Id, data.Quantity);
#endif
			var storage = _shared.Storage.Values;
			foreach (var entry in storage) {
				if (entry.Id != data.Id || entry.Quantity != data.Quantity)
					continue;

				storage.Remove(entry);
				_shared.StorageEntity.ReplaceStorage(storage);
				break;
			}
		}

		public bool Has(ItemId itemId) {
			foreach (var storageEntry in _shared.Storage.Values)
				if (storageEntry.Id == itemId)
					return true;
			return false;
		}
	}
}