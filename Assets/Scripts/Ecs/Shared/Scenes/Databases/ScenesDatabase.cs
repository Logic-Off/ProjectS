using Installers;
using Utopia;

namespace Ecs.Shared {
	public interface IScenesDatabase : IDatabase<LocationId, SceneData> { }

	[InstallerGenerator("Project")]
	public sealed class ScenesDatabase : ADatabase<LocationId, SceneData>, IScenesDatabase {
		public ScenesDatabase(ScenesDatabaseAsset database) {
			foreach (var data in database.Scenes)
				Add(data.Id, data);
		}
	}
}