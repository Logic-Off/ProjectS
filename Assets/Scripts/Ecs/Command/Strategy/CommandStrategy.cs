using Common;
using Utopia;

namespace Ecs.Command {
	public interface IPlannerCommand : IStrategyCommand<CommandEntity> { }

	[InstallerGenerator(InstallerId.Game)]
	public class CommandStrategy : AStrategy<CommandEntity, IPlannerCommand> {
		public override void Execute(CommandEntity entity) {
			foreach (var command in _commands) {
				if (!command.Accept(entity))
					continue;
				command.Apply(entity);
				break;
			}
		}
	}
}