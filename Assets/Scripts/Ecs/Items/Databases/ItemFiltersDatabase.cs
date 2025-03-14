using Utopia;

namespace Ecs.Item {
	public interface IItemFiltersDatabase : IDatabase<EItemType, ItemFilterEntry> { }

	[InstallerGenerator(InstallerId.Game)]
	public sealed class ItemFiltersDatabase : ADatabase<EItemType, ItemFilterEntry>, IItemFiltersDatabase {
		public ItemFiltersDatabase(ItemsDatabaseAsset database) {
			foreach (var entry in database.ItemFilters)
				Add(entry.ContainerType, entry);
		}
	}
}