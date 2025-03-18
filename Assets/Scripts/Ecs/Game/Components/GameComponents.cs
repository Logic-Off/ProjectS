using System;
using UnityEngine;
using Utopia;
using Zentitas;

namespace Ecs.Game {
	[Game, Character, Unique]
	public sealed class PlayerComponent : IComponent { }

	[Game, Character]
	public sealed class NpcComponent : IComponent { }

	[Game, Unique]
	public sealed class CameraComponent : IComponent { }

	[Game]
	public sealed class AnimatorComponent : IComponent {
		public Animator Value;
	}

	[Game]
	public sealed class AttackLoopedCastComponent : IComponent { }

	[Game]
	public sealed class GunFirePositionComponent : IComponent, IDisposable {
		public Transform Value;

		public void Dispose() => Value = null;
	}

	[Game]
	public sealed class GameTypeComponent : IComponent {
		public EGameType Value;
	}

	[Game]
	public sealed class EndTimeComponent : IComponent {
		public float Value;
	}

	[Game]
	public sealed class ReturnedToPoolComponent : IComponent { }

	[Game, Event(InstallerId.Game, EEventType.Added)]
	public sealed class RigidbodyForceComponent : IComponent {
		public float Power;
		public ForceMode Mode;
	}
}