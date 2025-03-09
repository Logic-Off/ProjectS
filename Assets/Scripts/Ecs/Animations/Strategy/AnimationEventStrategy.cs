using System.Collections.Generic;
using Common;
using Utopia;
using Zenject;

namespace Ecs.Animations {
	public interface IAnimationEventCommand : IStrategyCommand<AnimationEventEntity> {
		EAnimationEvent Name { get; }
	}

	[InstallerGenerator(InstallerId.Game)]
	public class AnimationEventStrategy : AStrategy<AnimationEventEntity, IAnimationEventCommand>, IInitializable {
		private readonly Dictionary<EAnimationEvent, IAnimationEventCommand> _dictionary = new();

		public void Initialize() {
			foreach (var command in _commands)
				_dictionary.Add(command.Name, command);
		}

		public override void Execute(AnimationEventEntity entity) {
			var command = _dictionary[entity.EventName];
			if (command.Accept(entity))
				command.Apply(entity);
		}
	}
}