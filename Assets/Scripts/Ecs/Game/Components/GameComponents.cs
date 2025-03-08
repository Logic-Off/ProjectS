using ProjectDawn.Navigation.Hybrid;
using UnityEngine;
using UnityEngine.AI;
using Zentitas;

namespace Ecs.Game {
	[Game]
	public sealed class NavmeshAgentComponent : IComponent {
		public NavMeshAgent Value;
	}

	[Game]
	public sealed class AuthoringAgentComponent : IComponent {
		public AgentAuthoring Value;
	}

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
}