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
}