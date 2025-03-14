using System.Collections.Generic;
using Ecs.Character;
using Ecs.Common;
using Ecs.Inventory;
using Utopia;

namespace Ecs.Item {
	[InstallerGenerator(InstallerId.Game)]
	public class ItemsFactory : IItemsFactory {
		private readonly IItemsDatabase _database;
		private readonly ItemInstanceIdGenerator _instanceIdGenerator;
		private readonly ItemContext _items;
		private readonly List<IItemBuilder> _builders;

		public ItemsFactory(
			ItemContext items,
			IItemsDatabase database,
			ItemInstanceIdGenerator instanceIdGenerator,
			List<IItemBuilder> builders
		) {
			_items = items;
			_database = database;
			_instanceIdGenerator = instanceIdGenerator;
			_builders = builders;
		}

		public ItemEntity Create(ItemId itemId) {
			var id = _instanceIdGenerator.Next();
			var entry = _database.Get(itemId);

			var item = _items.CreateEntity();
			item.AddId(IdGenerator.GetNext());
			item.AddItemInstanceId(id);
			item.AddItemId(itemId);
			item.AddItemType(entry.Type);
			item.AddStackSize(entry.StackSize);
			item.AddQuantity(1);
			item.IsStackable = entry.IsStackable;
			item.AddBuffs(new List<ABuff>());

			foreach (var builder in _builders)
				builder.Build(item);

			return item;
		}

		public void Destroy(ItemInstanceId id) {
			var item = _items.GetEntityWithItemInstanceId(id);
			item.IsDestroyed = true;
		}
	}
}