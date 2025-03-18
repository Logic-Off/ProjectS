using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public class SetPositionSystem : ReactiveSystem<GameEntity> {
		public SetPositionSystem(GameContext game) : base(game) { }

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
			=> context.CreateCollector(GameMatcher.NewPosition);

		protected override bool Filter(GameEntity entity)
			=> entity.HasNewPosition;

		protected override void Execute(List<GameEntity> entities) {
			foreach (var entity in entities) {
				var position = entity.NewPosition.Value;
				if (entity.HasNavmeshAgent)
					entity.NavmeshAgent.Value.Warp(position);
				else if(entity.HasTransform)
					entity.Transform.Value.position = position;
			}
		}
	}
}