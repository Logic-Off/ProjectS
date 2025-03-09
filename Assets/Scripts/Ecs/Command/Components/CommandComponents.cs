using System.Collections.Generic;
using Common;
using Ecs.Common;
using UnityEngine;
using Zentitas;
using AnimationEvent = Ecs.Animations.AnimationEvent;

namespace Ecs.Command {
	[Command]
	public sealed class StateComponent : IComponent {
		public ECommandState Value;
	}

	[Command]
	public sealed class StateNameComponent : IComponent {
		public EState Value;
	}

	[Command]
	public sealed class CommandTypeComponent : IComponent {
		public ECommandType Value;
	}

	[Command]
	public sealed class AnimationComponent : IComponent {
		public string Value;
	}

	[Command]
	public sealed class AbilityComponent : IComponent {
		public Id Value;
	}

	[Command]
	public sealed class DurationComponent : IComponent {
		public float Value;
	}

	[Command]
	public sealed class PauseComponent : IComponent {
		public float BeginAt;
		public float Duration;
		public bool Complete => Time.realtimeSinceStartup - BeginAt > Duration;
		public float EstimatePercentage => ((Time.realtimeSinceStartup - BeginAt) / Duration).Clamp01();
	}

	[Command]
	public sealed class StandingCommandComponent : IComponent {
		public override string ToString() => "StandingCommand";
	}

	[Command]
	public sealed class CurretnAnimationTimeComponent : IComponent {
		public float Value;
	}

	[Command]
	public sealed class AnimationEventsComponent : IComponent {
		public List<AnimationEvent> Values;
	}

	[Command]
	public sealed class StartTimeComponent : IComponent {
		public float Value;
	}

	[Command]
	public sealed class CasterComponent : IComponent {
		public Id Value;
	}

	[Command, AnimationEvent]
	public sealed class TargetComponent : IComponent {
		public Id Value;
	}

	[Command]
	public sealed class SpeedComponent : IComponent {
		public float Value;
	}

	[Game]
	public sealed class CurrentCommandComponent : IComponent {
		public Id Value;
	}
}