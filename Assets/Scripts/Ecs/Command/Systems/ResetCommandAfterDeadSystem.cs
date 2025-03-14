using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.Command {
	[InstallerGenerator(InstallerId.Game, 10_000_000)]
	public class ResetCommandAfterDeadSystem : ReactiveSystem<GameEntity> {
		private readonly CommandContext _command;

		public ResetCommandAfterDeadSystem(GameContext game, CommandContext command) : base(game) => _command = command;

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
			=> context.CreateCollector(GameMatcher.Dead.Added());

		protected override bool Filter(GameEntity entity)
			=> entity.IsDead && entity.HasCurrentCommand;

		protected override void Execute(List<GameEntity> entities) {
			foreach (var agent in entities) {
				var command = _command.GetEntityWithId(agent.CurrentCommand.Value);
				command.IsDestroyed = true;
			}
		}
	}
}