using UnityEngine;
using Zentitas;

namespace Ecs.Common {
	[Game, Structure, Character, Ability, Command, AnimationEvent]
	public sealed class IdComponent : IComponent {
		[PrimaryEntityIndex] public Id Value;
	}

	[Game]
	public sealed class InstanceIdComponent : IComponent {
		[PrimaryEntityIndex] public int Value;
	}

	[Game, Structure]
	public sealed class NameComponent : IComponent {
		[PrimaryEntityIndex] public string Value;
	}

	[Game, Structure]
	public sealed class PositionComponent : IComponent {
		public Vector3 Value;
	}

	[Game, Structure]
	public sealed class NewPositionComponent : IComponent {
		public Vector3 Value;
	}

	[Game, Structure]
	public sealed class RotationComponent : IComponent {
		public Quaternion Value;
	}

	[Game, Structure]
	public sealed class NewRotationComponent : IComponent {
		public Quaternion Value;
	}

	[Game, Structure]
	public sealed class PrefabComponent : IComponent {
		public string Value;
	}

	[Game, Structure]
	public sealed class TransformComponent : IComponent {
		public Transform Value;
	}

	[Game]
	public sealed class ColliderComponent : IComponent {
		public Collider Value;
	}

	[Ability, Command, AnimationEvent]
	public sealed class OwnerComponent : IComponent {
		[EntityIndex] public Id Value;
	}

	[Command, AnimationEvent]
	public sealed class DestroyedComponent : IComponent { }
}