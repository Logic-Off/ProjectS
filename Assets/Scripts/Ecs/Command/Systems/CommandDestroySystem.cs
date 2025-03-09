using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.Command {
	[InstallerGenerator(InstallerId.Game)]
	public class CommandDestroySystem : ReactiveSystem<CommandEntity> {
		private readonly CommandContext _command;

		public CommandDestroySystem(CommandContext command) : base(command) => _command = command;

		protected override ICollector<CommandEntity> GetTrigger(IContext<CommandEntity> context)
			=> context.CreateCollector(CommandMatcher.Destroyed.Added());

		protected override bool Filter(CommandEntity entity)
			=> entity.IsDestroyed;

		protected override void Execute(List<CommandEntity> entities) {
			foreach (var entity in entities) {
				if (entity.HasId) {
					if (entity.HasAnimationEvents) {
						entity.AnimationEvents.Values.ReturnToPool();
						entity.RemoveAnimationEvents();
					}

					var others = _command.GetEntitiesWithOwner(entity.Id.Value);
					foreach (var other in others)
						other.IsDestroyed = true;
				}
				
				entity.Destroy();
			}
		}
	}
}