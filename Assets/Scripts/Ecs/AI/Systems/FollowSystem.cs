using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.AI {
	[InstallerGenerator(InstallerId.Game)]
	public class FollowSystem : ReactiveSystem<GameEntity> {
		public FollowSystem(GameContext game) : base(game) { }

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
			=> context.CreateCollector(GameMatcher.Destination.Added());

		protected override bool Filter(GameEntity entity)
			=> entity.HasDestination;

		protected override void Execute(List<GameEntity> entities) {
			foreach (var entity in entities) {
				if (!entity.HasAuthoringAgent) {
					Clear(entity);
					continue;
				}

				entity.AuthoringAgent.Value.SetDestination(entity.Destination.Value);
				entity.IsMoving = true;
			}
		}

		private static void Clear(GameEntity entity) {
			if (entity.HasDestination)
				entity.RemoveDestination();
			entity.IsMoving = false;
			entity.IsDestinationReached = true;
		}
	}
}