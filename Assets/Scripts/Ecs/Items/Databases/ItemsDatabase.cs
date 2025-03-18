using Utopia;

namespace Ecs.Item {
	public interface IItemsDatabase : IDatabase<ItemId, ItemData> { }

	[InstallerGenerator(InstallerId.Inventory)]
	public sealed class ItemsDatabase : ADatabase<ItemId, ItemData>, IItemsDatabase {
		public ItemsDatabase(ItemsDatabaseAsset database) {
			foreach (var entry in database.All)
				Add(entry.Id, entry);
		}
	}
}