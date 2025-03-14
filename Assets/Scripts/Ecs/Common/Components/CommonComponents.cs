using UnityEngine;
using Zentitas;

namespace Ecs.Common {
	[Game, Structure, Character, Ability, Command, AnimationEvent, Ui, Item]
	public sealed class IdComponent : IComponent {
		[PrimaryEntityIndex] public Id Value;
	}

	[Game, Structure, Ui]
	public sealed class InstanceIdComponent : IComponent {
		[PrimaryEntityIndex] public int Value;
	}

	[Game, Structure, Ui]
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

	[Game, Structure, Ui]
	public sealed class PrefabComponent : IComponent {
		public string Value;
	}

	[Game, Structure]
	public sealed class TransformComponent : IComponent {
		public Transform Value;
	}

	[Game, Structure]
	public sealed class ColliderComponent : IComponent {
		public Collider Value;
	}

	[Ability, Command, AnimationEvent, Inventory, Item]
	public sealed class OwnerComponent : IComponent {
		[EntityIndex] public Id Value;
	}

	[Ui]
	public sealed class ParentComponent : IComponent {
		[EntityIndex] public Id Value;
	}

	[Command, AnimationEvent, Item]
	public sealed class DestroyedComponent : IComponent { }
}