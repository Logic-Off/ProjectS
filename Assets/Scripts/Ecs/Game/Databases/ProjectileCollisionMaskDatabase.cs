using Ecs.Character;
using Utopia;

namespace Ecs.Game {
	public interface IProjectileCollisionMaskDatabase : IDatabase<ETeam, ProjectileCollisionMaskData> { }

	[InstallerGenerator(InstallerId.Game)]
	public sealed class ProjectileCollisionMaskDatabase : ADatabase<ETeam, ProjectileCollisionMaskData>, IProjectileCollisionMaskDatabase {
		public ProjectileCollisionMaskDatabase(GameCollisionDatabaseAsset database) {
			foreach (var data in database.ProjectileCollisionMaskList)
				Add(data.Team, data);
		}
	}
}