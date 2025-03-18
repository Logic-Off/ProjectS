using UnityEngine;
using UnityEngine.AI;

namespace Ecs.Game {
	public sealed class NavMeshAgentGSubBehaviour : AGameSubBehaviour {
		[SerializeField] private NavMeshAgent _agent;
		
		public override void Link(GameEntity entity) => entity.AddNavmeshAgent(_agent);
	}
}