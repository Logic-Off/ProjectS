using System.Collections.Generic;
using Common;
using Ecs.Ability;
using Utopia;
using Zentitas;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public class AttackTargetsSystem : ReactiveSystem<GameEntity> {
		private readonly GameContext _game;
		private readonly AbilityContext _ability;

		public AttackTargetsSystem(GameContext game, AbilityContext ability) : base(game) {
			_game = game;
			_ability = ability;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
			=> context.CreateCollector(GameMatcher.HostileTargets);

		protected override bool Filter(GameEntity entity)
			=> entity.HasHostileTargets;

		protected override void Execute(List<GameEntity> entities) {
			foreach (var agent in entities) {
				var attackTargets = agent.AttackTargets.Values;
				attackTargets.Clear();
				var abilities = ListPool<AbilityEntity>.Get();
				OnCollectAbilities(agent, abilities);
				OnCollectAttackTargets(agent, abilities, attackTargets);

				agent.ReplaceAttackTargets(attackTargets);
			}
		}

		private void OnCollectAbilities(GameEntity agent, List<AbilityEntity> abilities) {
			var allAgentAbilities = _ability.GetEntitiesWithOwner(agent.Id.Value);
			foreach (var ability in allAgentAbilities) {
				if (ability.AbilityType.Value is not EAbilityType.Attack)
					continue;
				abilities.Add(ability);
			}
		}

		private void OnCollectAttackTargets(GameEntity agent, List<AbilityEntity> abilities, List<TargetData> attackTargets) {
			foreach (var entry in agent.HostileTargets.Values) {
				var target = _game.GetEntityWithId(entry.Id);
				if (target == null)
					continue;

				var distance = agent.Position.Value.Distance(entry.LastPosition);

				foreach (var ability in abilities) {
					if (!ability.CanCastByDistance(distance))
						continue;

					attackTargets.Add(entry);
					break;
				}
			}
		}
	}
}