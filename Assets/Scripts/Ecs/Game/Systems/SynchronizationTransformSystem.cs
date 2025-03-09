using Utopia;
using Zentitas;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public class SynchronizationTransformSystem : IFixedUpdateSystem {
		private readonly IGroup<GameEntity> _group;

		public SynchronizationTransformSystem(GameContext game) => _group = game.GetGroup(GameMatcher.AllOf(GameMatcher.Transform).NoneOf(GameMatcher.Dead));

		public void FixedUpdate() {
			var list = ListPool<GameEntity>.Get();
			_group.GetEntities(list);

			foreach (var entity in list) {
				var transform = entity.Transform.Value;
				var position = transform.position;
				var rotation = transform.rotation;
				if (position != entity.Position.Value)
					entity.ReplacePosition(position);
				if (rotation != entity.Rotation.Value)
					entity.ReplaceRotation(rotation);
			}

			list.ReturnToPool();
		}
	}
}