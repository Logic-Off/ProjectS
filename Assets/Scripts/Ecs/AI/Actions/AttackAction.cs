using Ecs.Ability;
using Ecs.Command;
using Utopia;

namespace Ecs.AI {
	[InstallerGenerator(InstallerId.Game)]
	public class AttackAction : IAction {
		private readonly AbilityContext _ability;
		private readonly CommandContext _command;

		public AttackAction(AbilityContext ability, CommandContext command) {
			_ability = ability;
			_command = command;
		}

		public float GetScore(GameEntity entity) {
			if (!entity.HasPosition || !entity.HasAttackTarget)
				return 0f;

			var abilities = _ability.GetEntitiesWithOwner(entity.Id.Value);
			foreach (var abilityEntity in abilities) {
				if (abilityEntity.AbilityType.Value is not EAbilityType.Attack || abilityEntity.IsCooldown)
					continue;
				var distance = abilityEntity.Parameters.Values[EAbilityParameter.Distance];
				// Если чем-то можем ударить
				if (entity.AttackTarget.Value.Distance < distance)
					return 100f;
			}

			return 0f;
		}

		public void Execute(GameEntity agent) {
			if (agent.HasCurrentCommand)
				return;

			var abilities = _ability.GetEntitiesWithOwner(agent.Id.Value);
			foreach (var abilityEntity in abilities) {
				if (abilityEntity.AbilityType.Value is not EAbilityType.Attack || abilityEntity.IsCooldown)
					continue;
				var distance = abilityEntity.Parameters.Values[EAbilityParameter.Distance];
				// Если чем-то можем ударить
				if (agent.AttackTarget.Value.Distance < distance) {
					D.Error("[AttackAction]", "Кастуем абилку ", abilityEntity.Id.Value);
					var command = _command.Create(agent.Id.Value);
					command.AddCommandType(ECommandType.Ability);
					command.AddTarget(agent.AttackTarget.Value.Id);
					command.AddAbility(abilityEntity.Id.Value);
					command.IsStandingCommand = abilityEntity.IsStandingCast;
					return;
				}
			}
		}
	}
}