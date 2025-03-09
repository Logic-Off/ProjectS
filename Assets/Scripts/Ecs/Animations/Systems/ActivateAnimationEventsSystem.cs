using Common;
using Ecs.Command;
using Ecs.Common;
using Utopia;
using Zentitas;

namespace Ecs.Animations {
	[InstallerGenerator(InstallerId.Game)]
	public class ActivateAnimationEventsSystem : IUpdateSystem {
		private readonly IGroup<CommandEntity> _group;

		private readonly AnimationEventContext _animationEvent;
		private readonly AnimationEventStrategy _animationEventStrategy;
		private readonly IClock _clock;

		public ActivateAnimationEventsSystem(
			CommandContext command,
			AnimationEventContext animationEvent,
			AnimationEventStrategy animationEventStrategy,
			IClock clock
		) {
			_animationEvent = animationEvent;
			_animationEventStrategy = animationEventStrategy;
			_clock = clock;
			_group = command.GetGroup(
				CommandMatcher.AllOf(CommandMatcher.AnimationEvents, CommandMatcher.StartTime, CommandMatcher.State)
					.NoneOf(CommandMatcher.Destroyed)
			);
		}

		public void Update() {
			var commands = ListPool<CommandEntity>.Get();
			var removed = ListPool<AnimationEvent>.Get();
			_group.GetEntities(commands);
			foreach (var command in commands) {
				if (command.State.Value is ECommandState.Success or ECommandState.Failed)
					continue;

				foreach (var animationEvent in command.AnimationEvents.Values) {
					var eventTime = animationEvent.Time / command.Speed.Value;
					var currentTime = _clock.Time - command.StartTime.Value;
					if (eventTime > currentTime)
						continue;

					CreateAnimationEvent(animationEvent._data, command);
					removed.Add(animationEvent);
				}

				command.AnimationEvents.Values.Remove(ref removed);
			}

			removed.ReturnToPool();
			commands.ReturnToPool();
		}

		private void CreateAnimationEvent(AnimationEventData data, CommandEntity entity) {
			var animationEvent = _animationEvent.CreateEntity();
			animationEvent.AddId(IdGenerator.GetNext());
			animationEvent.AddOwner(entity.Caster.Value);
			animationEvent.AddEventName(data.EventName);
			if (entity.HasTarget)
				animationEvent.AddTarget(entity.Target.Value);

			_animationEventStrategy.Execute(animationEvent);
			animationEvent.IsDestroyed = true;
		}
	}
}