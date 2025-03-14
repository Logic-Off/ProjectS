using Utopia;

namespace Ecs.Item {
	public interface IItemsDatabase : IDatabase<ItemId, ItemEntry> { }

	[InstallerGenerator(InstallerId.Inventory)]
	public sealed class ItemsDatabase : ADatabase<ItemId, ItemEntry>, IItemsDatabase {
		public ItemsDatabase(ItemsDatabaseAsset database) {
			foreach (var entry in database.All)
				Add(entry.Id, entry);
		}
	}
}