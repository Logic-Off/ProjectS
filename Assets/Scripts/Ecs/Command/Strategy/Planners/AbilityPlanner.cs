using Ecs.Common;
using Utopia;

namespace Ecs.Command {
	[InstallerGenerator(InstallerId.Game)]
	public class AbilityPlanner : IPlannerCommand {
		private readonly CommandContext _command;
		public AbilityPlanner(CommandContext command) => _command = command;
		public bool Accept(CommandEntity entity) => entity.CommandType.Value == ECommandType.Ability;

		public void Apply(CommandEntity agent) {
			var ability = _command.CreateState(agent, EState.Ability);
			ability.AddAbility(agent.Ability.Value);
			ability.AddCaster(agent.Owner.Value);

			if (agent.HasTarget && agent.Target.Value != Id.None)
				ability.AddTarget(agent.Target.Value);
		}
	}
}