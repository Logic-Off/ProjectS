using Utopia;

namespace Ecs.Character {
	public interface IBuffDatabase : IDatabase<BuffId, BuffData> { }

	[InstallerGenerator(InstallerId.Game)]
	public sealed class BuffDatabase : ADatabase<BuffId, BuffData>, IBuffDatabase {
		public BuffDatabase(CharacterDatabaseAsset database) {
			foreach (var entry in database.Buffs)
				Add(entry.Id, entry);
		}
	}
}