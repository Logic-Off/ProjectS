using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.Command {
	[InstallerGenerator(InstallerId.Game)]
	public class CommandPlannerSystem : ReactiveSystem<CommandEntity> {
		private readonly CommandContext _command;
		private readonly CommandStrategy _planner;

		public CommandPlannerSystem(
			CommandContext command,
			CommandStrategy planner
		) : base(command) {
			_command = command;
			_planner = planner;
		}

		protected override ICollector<CommandEntity> GetTrigger(IContext<CommandEntity> context)
			=> context.CreateCollector(CommandMatcher.CommandType.Added());

		protected override bool Filter(CommandEntity entity)
			=> entity.HasCommandType;

		protected override void Execute(List<CommandEntity> entities) {
			foreach (var entity in entities) {
				var otherCommands = _command.GetEntitiesWithOwner(entity.Owner.Value);
				foreach (var command in otherCommands) {
					if (command.Id.Value == entity.Id.Value)
						continue;
					command.IsDestroyed = true;
				}

				_planner.Execute(entity);
			}
		}
	}
}