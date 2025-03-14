using System;
using System.Collections.Generic;
using Zentitas;

namespace Ecs.Character {
	[Character]
	public sealed class ParametersComponent : IComponent {
		public CharacterParameters Value;
	}

	[Character]
	public sealed class HealthComponent : IComponent {
		public float Value;
	}

	[Character]
	public sealed class MaxHealthComponent : IComponent {
		public float Value;
	}

	[Character]
	public sealed class HealthModifierComponent : IComponent, IDisposable {
		public List<StatModifier> Values;

		public void Dispose() => Values.Clear();
	}

	[Character]
	public sealed class AttackComponent : IComponent {
		public float Value;
	}

	[Character]
	public sealed class CastSpeedComponent : IComponent {
		public float Value;
	}

	[Character]
	public sealed class MovementSpeedComponent : IComponent {
		public float Value;
	}

	[Character]
	public sealed class VisionRangeComponent : IComponent {
		public float Value;
	}

	[Character]
	public sealed class VisionAngleComponent : IComponent {
		public float Value;
	}

	[Character]
	public sealed class StealthComponent : IComponent {
		public float Value;
	}

	[Character]
	public sealed class ObservationComponent : IComponent {
		public float Value;
	}

	[Character]
	public sealed class DeadComponent : IComponent { }

	[Character, Item]
	public sealed class BuffsComponent : IComponent {
		public List<ABuff> Values;
	}

	[Character, Item]
	public sealed class BuffModifierComponent : IComponent {
		public List<BuffModifier> Values;
	}
}