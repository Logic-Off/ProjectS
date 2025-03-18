using Ecs.Common;
using Utopia;

namespace Ecs.Command {
	[InstallerGenerator(InstallerId.Game)]
	public class AbilityPlanner : IPlannerCommand {
		private readonly CommandContext _command;
		public AbilityPlanner(CommandContext command) => _command = command;
		public bool Accept(CommandEntity command) => command.CommandType.Value == ECommandType.Ability;

		public void Apply(CommandEntity animationEvent) {
			var ability = _command.CreateState(animationEvent, EState.Ability);

			ability.AddAbility(animationEvent.Ability.Value);
			ability.AddCaster(animationEvent.Owner.Value);

			if (animationEvent.HasTarget && animationEvent.Target.Value != Id.None)
				ability.AddTarget(animationEvent.Target.Value);

			_command.CreateState(animationEvent, EState.ExitAbility);
		}
	}
}