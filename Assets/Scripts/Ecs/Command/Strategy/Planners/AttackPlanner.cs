using Utopia;

namespace Ecs.Command {
	[InstallerGenerator(InstallerId.Game)]
	public class AttackPlanner : IPlannerCommand {
		private readonly CommandContext _command;
		public AttackPlanner(CommandContext command) => _command = command;
		public bool Accept(CommandEntity command) => command.CommandType.Value == ECommandType.Attack;

		public void Apply(CommandEntity animationEvent) {
			var ability = _command.CreateState(animationEvent, EState.Attack);

			ability.AddAbility(animationEvent.Ability.Value);
			ability.AddCaster(animationEvent.Owner.Value);

			_command.CreateState(animationEvent, EState.ExitAbility);
		}
	}
}