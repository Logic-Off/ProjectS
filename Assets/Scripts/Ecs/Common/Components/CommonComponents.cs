using UnityEngine;
using Zentitas;

namespace Ecs.Common {
	[Game, Structure]
	public sealed class IdComponent : IComponent {
		[PrimaryEntityIndex] public Id Value;
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
}