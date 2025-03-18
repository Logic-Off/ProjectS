using Utopia;

namespace Ecs.Item {
	public interface IContainerSettingsDatabase : IDatabase<string, ContainerSettingsData> { }

	[InstallerGenerator(InstallerId.Game)]
	public sealed class ContainerSettingsDatabase : ADatabase<string, ContainerSettingsData>, IContainerSettingsDatabase {
		public ContainerSettingsDatabase(ItemsDatabaseAsset database) {
			foreach (var entry in database.ContainerSettings)
				Add(entry.Id, entry);
		}
	}
}