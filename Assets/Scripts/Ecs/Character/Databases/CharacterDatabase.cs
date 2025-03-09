using Utopia;

namespace Ecs.Character {
	public interface ICharacterDatabase : IDatabase<string, CharacterData> { }

	[InstallerGenerator(InstallerId.Game)]
	public sealed class CharacterDatabase : ADatabase<string, CharacterData>, ICharacterDatabase {
		public CharacterDatabase(CharacterDatabaseAsset database) {
			foreach (var entry in database.Characters)
				Add(entry.Name, entry);
		}
	}
}