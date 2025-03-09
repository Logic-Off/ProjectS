using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public class HostileTargetSystem : ReactiveSystem<GameEntity> {
		public HostileTargetSystem(GameContext game) : base(game) { }

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
			=> context.CreateCollector(GameMatcher.HostileTargets);

		protected override bool Filter(GameEntity entity)
			=> entity.HasHostileTargets;

		protected override void Execute(List<GameEntity> entities) {
			foreach (var entity in entities) {
				if (entity.HostileTargets.Values.Count > 0) {
					if (entity.HasHostileTarget && entity.HostileTargets.Values.Contains(entity.HostileTarget.Value))
						continue;

					// Берется первая цель, в дальнейшем можно реализовать систему агра
					var target = entity.HostileTargets.Values[0];
					entity.ReplaceHostileTarget(target);
					entity.ReplaceLastHostileTarget(target);
				} else if (entity.HasHostileTarget)
					entity.RemoveHostileTarget();
			}
		}
	}
}