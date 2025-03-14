using Ecs.Inventory;
using Utopia;

namespace Ecs.Item {
	public interface IContainerFiltersDatabase : IDatabase<EContainerType, ContainerFilterEntry> { }

	[InstallerGenerator(InstallerId.Game)]
	public sealed class ContainerFiltersDatabase : ADatabase<EContainerType, ContainerFilterEntry>, IContainerFiltersDatabase {
		public ContainerFiltersDatabase(ItemsDatabaseAsset database) {
			foreach (var entry in database.ContainerItemFilters)
				Add(entry.ContainerType, entry);
		}
	}
}