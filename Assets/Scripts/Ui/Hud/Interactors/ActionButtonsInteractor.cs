using Ecs.Ability;
using Ecs.Command;
using Ecs.Game;
using Utopia;

namespace Ui.Hud {
	[InstallerGenerator(InstallerId.Ui)]
	public class ActionButtonsInteractor {
		private readonly GameContext _game;
		private readonly AbilityContext _ability;
		private readonly CommandContext _command;

		public ActionButtonsInteractor(GameContext game, AbilityContext ability, CommandContext command) {
			_game = game;
			_ability = ability;
			_command = command;
		}

		public void CastDefaultAbility() {
			var player = _game.PlayerEntity;
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
			
			var target = _game.GetEntityWithId(player.AttackTarget.Value.Id);
			player.LookAt(target);

			D.Error("[ActionButtonsInteractor]", "Применяем абилку");
			var command = _command.Create(player.Id.Value);
			command.AddCommandType(ECommandType.Ability);
			command.AddTarget(player.AttackTarget.Value.Id);
			command.AddAbility(baseAbility.Id.Value);
			command.IsStandingCommand = baseAbility.IsStandingCast;
		}
	}
}