using Utopia;

namespace Ecs.Common {
	public interface IPrefabsDatabase : IDatabase<string, PrefabData> { }

	[InstallerGenerator("Game")]
	public sealed class PrefabsDatabase : ADatabase<string, PrefabData>, IPrefabsDatabase {
		public PrefabsDatabase(PrefabsDatabaseAsset database) {
			foreach (var data in database.All)
				Add(data.Name, data);
		}
	}
}