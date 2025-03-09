using Utopia;

namespace Ecs.Animations {
	public interface IAnimationsDatabase : IDatabase<string, AnimationData> { }
	[InstallerGenerator(InstallerId.Game)]
	public sealed class AnimationsDatabase : ADatabase<string, AnimationData>, IAnimationsDatabase {
		public AnimationsDatabase(AnimationsDatabaseAsset database) {
			foreach (var entry in database.All)
				Add(entry.Id, entry);
		}
	}
}