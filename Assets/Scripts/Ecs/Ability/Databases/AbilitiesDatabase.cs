using Utopia;

namespace Ecs.Ability {
	public interface IAbilitiesDatabase : IDatabase<AbilityId, AbilityData> { }

	[InstallerGenerator(InstallerId.Game)]
	public sealed class AbilitiesDatabase : ADatabase<AbilityId, AbilityData>, IAbilitiesDatabase {
		public AbilitiesDatabase(AbilitiesDatabaseAsset database) {
			foreach (var entry in database.Abilities)
				Add(entry.Id, entry);
		}
	}
}