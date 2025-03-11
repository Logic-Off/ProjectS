using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.AI {
	[InstallerGenerator(InstallerId.Game)]
	public class MovingSystem : ReactiveSystem<GameEntity> {
		public MovingSystem(GameContext game) : base(game) { }

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
			=> context.CreateCollector(GameMatcher.NavMeshPath);

		protected override bool Filter(GameEntity entity)
			=> entity.HasNavMeshPath && entity.NavMeshPath.Value.corners.Length > 0;

		protected override void Execute(List<GameEntity> entities) {
			foreach (var entity in entities)
				entity.IsMoving = true;
		}
	}
}