using System.Collections.Generic;
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

	[Game]
	public sealed class NavMeshPathComponent : IComponent {
		public NavMeshPath Value;
	}

	[Game]
	public sealed class MovingComponent : IComponent { }

	[Game, Structure]
	public sealed class WaypointsComponent : IComponent {
		public List<WaypointEntry> Values;
	}

	[Game]
	public sealed class WaypointIndexComponent : IComponent {
		public int Value;
	}

	[Game]
	public sealed class DestinationComponent : IComponent {
		public Vector3 Value;
	}

	[Game]
	public sealed class DestinationReachedComponent : IComponent { }

	[Game]
	public sealed class TargetDestinationComponent : IComponent {
		public Vector3 Value;
	}
}