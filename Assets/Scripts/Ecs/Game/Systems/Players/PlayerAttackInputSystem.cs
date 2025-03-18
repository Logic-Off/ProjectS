using System.Collections.Generic;
using Ecs.Ability;
using Ecs.Command;
using Utopia;
using Zentitas;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public sealed class PlayerAttackInputSystem : ReactiveSystem<InputEntity> {
		private readonly GameContext _game;
		private readonly AbilityContext _ability;
		private readonly CommandContext _command;

		public PlayerAttackInputSystem(InputContext input, GameContext game, AbilityContext ability, CommandContext command) : base(input) {
			_game = game;
			_ability = ability;
			_command = command;
		}

		protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
			=> context.CreateCollector(InputMatcher.AttackPressed.AddedOrRemoved());

		protected override bool Filter(InputEntity entity)
			=> true;

		protected override void Execute(List<InputEntity> entities) {
			foreach (var entity in entities) {
				var player = _game.PlayerEntity;
				player.IsAttackLoopedCast = entity.IsAttackPressed;

				if (!entity.IsAttackPressed)
					return;

				if (!player.HasBaseAbility) {
					D.Error("[ActionButtonsInteractor]", "Нет абилки");
					return;
				}

				if (!player.HasAttackTarget) {
					D.Error("[ActionButtonsInteractor]", "Нет цели");
					return;
				}

				if (player.HasCurrentCommand) {
					D.Error("[ActionButtonsInteractor]", "Выполняет команду");
					return;
				}

				var baseAbility = _ability.GetAbility(player.Id.Value, player.BaseAbility.Value);
				if (baseAbility == null) {
					D.Error("[ActionButtonsInteractor]", "Сущность абилки не найдена");
					return;
				}

				D.Error("[ActionButtonsInteractor]", "Применяем абилку");
				var command = _command.Create(player.Id.Value);
				command.AddCommandType(ECommandType.Attack);
				command.AddTarget(player.AttackTarget.Value.Id);
				command.AddAbility(baseAbility.Id.Value);
				command.IsStandingCommand = baseAbility.IsStandingCast;
			}
		}
	}
}