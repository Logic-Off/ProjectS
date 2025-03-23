using System.Collections.Generic;
using Ecs.Common;
using Zentitas;

namespace Ecs.Animations {
	[AnimationEvent]
	public sealed class EventNameComponent : IComponent {
		public EAnimationEvent Value;
		public static implicit operator EAnimationEvent(EventNameComponent component) => component.Value;
		public override string ToString() => $"EventName[{Value}]";
	}

	[AnimationEvent]
	public sealed class FloatComponent : IComponent {
		public Dictionary<string, float> Values;
		public override string ToString() => $"FloatValues";
	}

	[AnimationEvent]
	public sealed class BoolComponent : IComponent {
		public Dictionary<string, bool> Values;
		public override string ToString() => $"BoolValues";
	}

	[AnimationEvent]
	public sealed class StringComponent : IComponent {
		public Dictionary<string, string> Values;
		public override string ToString() => $"StringValues";
	}

	[AnimationEvent]
	public sealed class AbilityComponent : IComponent {
		public Id Value;
	}
}