using ProjectDawn.Navigation.Hybrid;
using UnityEngine;

namespace Ecs.Game {
	public sealed class AuthoringAgentGSubBehaviour : AGameSubBehaviour {
		[SerializeField] private AgentAuthoring _agent;
		
		public override void Link(GameEntity entity) => entity.AddAuthoringAgent(_agent);
	}
}