using Ecs.Ability;
using Ecs.Command;
using Ecs.Game;
using Utopia;

namespace Ecs.AI {
	[InstallerGenerator(InstallerId.Game)]
	public class AttackAction : IAction {
		public EAiAction Name => EAiAction.Attack;
		private readonly AbilityContext _ability;
		private readonly CommandContext _command;
		private readonly GameContext _game;

		public AttackAction(AbilityContext ability, CommandContext command, GameContext game) {
			_ability = ability;
			_command = command;
			_game = game;
		}

		public float GetScore(GameEntity entity) {
			if (!entity.HasPosition || !entity.HasAttackTarget)
				return 0f;

			var abilities = _ability.GetEntitiesWithOwner(entity.Id.Value);
			foreach (var abilityEntity in abilities) {
				if (abilityEntity.AbilityType.Value is not EAbilityType.Attack)
					continue;
				var distance = abilityEntity.Parameters.Values[EAbilityParameter.Distance];
				// Если чем-то можем ударить
				if (entity.AttackTarget.Value.Distance < distance)
					return 100f;
			}

			// Считаем что это радиус для удара, пускай следит за игроком
			if (entity.AttackTarget.Value.Distance < 2)
				return 100f;

			return 0f;
		}

		public void Enter(GameEntity agent) { }

		public void Exit(GameEntity agent) { }

		public void Execute(GameEntity agent) {
			var target = GetTarget(agent);
			if (target == null)
				return;

			agent.LookAt(target);
			if (agent.HasCurrentCommand)
				return;

			var abilities = _ability.GetEntitiesWithOwner(agent.Id.Value);
			foreach (var abilityEntity in abilities) {
				if (abilityEntity.AbilityType.Value is not EAbilityType.Attack || abilityEntity.IsCooldown)
					continue;
				var distance = abilityEntity.Parameters.Values[EAbilityParameter.Distance];
				// Если чем-то можем ударить
				if (agent.AttackTarget.Value.Distance < distance) {
					var command = _command.Create(agent.Id.Value);
					command.AddCommandType(ECommandType.Ability);
					command.AddTarget(agent.AttackTarget.Value.Id);
					command.AddAbility(abilityEntity.Id.Value);
					command.IsStandingCommand = abilityEntity.IsStandingCast;
					return;
				}
			}
		}

		private GameEntity GetTarget(GameEntity entity) => entity.HasHostileTarget ? _game.GetEntityWithId(entity.HostileTarget.Value.Id) : null;
	}
}