using Utopia;

namespace Ecs.Item {
	public interface IContainerSettingsDatabase : IDatabase<string, ContainerSettingsEntry> { }

	[InstallerGenerator(InstallerId.Game)]
	public sealed class ContainerSettingsDatabase : ADatabase<string, ContainerSettingsEntry>, IContainerSettingsDatabase {
		public ContainerSettingsDatabase(ItemsDatabaseAsset database) {
			foreach (var entry in database.ContainerSettings)
				Add(entry.Id, entry);
		}
	}
}