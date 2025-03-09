using System.Collections.Generic;
using Ecs.Common;
using Utopia;
using Zentitas;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public class AttackTargetSystem : ReactiveSystem<GameEntity> {
		private readonly GameContext _game;

		public AttackTargetSystem(GameContext game) : base(game) {
			_game = game;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
			=> context.CreateCollector(GameMatcher.AttackTargets);

		protected override bool Filter(GameEntity entity)
			=> entity.HasAttackTargets;

		protected override void Execute(List<GameEntity> entities) {
			foreach (var entity in entities) {
				if (entity.AttackTargets.Values.Count > 0) {
					// Не меняем цель если она есть в списке
					// Убрал блок, цель не меняется по дистанции и это плохо
					// if (entity.HasAttackTarget && Contains(entity.AttackTarget.Value.Id, entity.AttackTargets.Values))
					// continue;

					var target = FindNearest(entity.AttackTargets.Values);
					if (entity.IsPlayer) {
						if (entity.HasAttackTarget) {
							var previousTarget = _game.GetEntityWithId(entity.AttackTarget.Value.Id);
							previousTarget.IsPlayerTarget = false;
						}

						var targetEntity = _game.GetEntityWithId(target.Id);
						targetEntity.IsPlayerTarget = true;
					}

					entity.ReplaceAttackTarget(target);
					entity.ReplaceLastAttackTarget(target);
				} else if (entity.HasAttackTarget) {
					if (entity.IsPlayer) {
						var previousTarget = _game.GetEntityWithId(entity.AttackTarget.Value.Id);
						previousTarget.IsPlayerTarget = false;
					}

					entity.RemoveAttackTarget();
				}
			}
		}

		private TargetData FindNearest(List<TargetData> targets) {
			var maxDistance = float.MaxValue;
			var targetIndex = 0;
			for (var index = 0; index < targets.Count; index++) {
				var entry = targets[index];
				if (entry.Distance >= maxDistance)
					continue;
				maxDistance = entry.Distance;
				targetIndex = index;
			}

			return targets[targetIndex];
		}

		private bool Contains(Id targetId, List<TargetData> targets) {
			foreach (var target in targets)
				if (target.Id == targetId)
					return true;

			return false;
		}
	}
}