using Utopia;
using Zentitas;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public class RemovePauseSystem : IUpdateSystem {
		private readonly IGroup<GameEntity> _group;
		public RemovePauseSystem(GameContext game) => _group = game.GetGroup(GameMatcher.Pause);

		public void Update() {
			var list = ListPool<GameEntity>.Get();
			_group.GetEntities(list);

			foreach (var entity in list)
				if (entity.Pause.Complete)
					entity.RemovePause();

			list.ReturnToPool();
		}
	}
}