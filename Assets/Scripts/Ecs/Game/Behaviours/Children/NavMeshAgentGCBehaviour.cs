using UnityEngine;
using UnityEngine.AI;

namespace Ecs.Game.Children {
	public sealed class NavMeshAgentGCBehaviour : AGameChildBehaviour {
		[SerializeField] private NavMeshAgent _agent;
		
		public override void Link(GameEntity entity) => entity.AddNavmeshAgent(_agent);
	}
}