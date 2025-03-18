using Utopia;

namespace Ecs.Item {
	public interface IItemFiltersDatabase : IDatabase<EItemType, ItemFilterData> { }

	[InstallerGenerator(InstallerId.Game)]
	public sealed class ItemFiltersDatabase : ADatabase<EItemType, ItemFilterData>, IItemFiltersDatabase {
		public ItemFiltersDatabase(ItemsDatabaseAsset database) {
			foreach (var entry in database.ItemFilters)
				Add(entry.ContainerType, entry);
		}
	}
}