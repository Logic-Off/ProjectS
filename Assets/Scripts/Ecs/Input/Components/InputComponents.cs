using UnityEngine;
using Zentitas;

namespace Ecs.Input {
	[Input, Unique]
	public sealed class PlayerInputComponent : IComponent { }

	[Input]
	public sealed class MovementComponent : IComponent {
		public Vector2 Value;
	}

	[Input]
	public sealed class RotateComponent : IComponent {
		public Vector2 Value;
	}

	[Input]
	public sealed class AttackPressedComponent : IComponent { }
}