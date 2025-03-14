using Utopia;

namespace Common {
	public interface IIconsDatabase : IDatabase<string, IconData> { }

	[InstallerGenerator(InstallerId.Game)]
	public sealed class IconsDatabase : ADatabase<string, IconData>, IIconsDatabase {
		public IconsDatabase(IconsDatabaseAsset database) {
			foreach (var entry in database.Icons)
				Add(entry.Id, entry);
		}
	}
}