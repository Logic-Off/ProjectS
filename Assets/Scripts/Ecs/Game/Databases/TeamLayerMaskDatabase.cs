using Ecs.Character;
using Utopia;

namespace Ecs.Game {
	public interface ITeamLayerMaskDatabase : IDatabase<ETeam, TeamLayerMaskData> { }

	[InstallerGenerator(InstallerId.Game)]
	public sealed class TeamLayerMaskDatabase : ADatabase<ETeam, TeamLayerMaskData>, ITeamLayerMaskDatabase {
		public TeamLayerMaskDatabase(GameCollisionDatabaseAsset database) {
			foreach (var data in database.TeamLayerMaskDataList)
				Add(data.Team, data);
		}
	}
}