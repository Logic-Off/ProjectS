using UnityEngine;
using UnityEngine.AI;

namespace Ecs.Game.Children {
	public sealed class NavMeshAgentGCBehaviour : AGameSubBehaviour {
		[SerializeField] private NavMeshAgent _agent;
		
		public override void Link(GameEntity entity) => entity.AddNavmeshAgent(_agent);
	}
}