using ProjectDawn.Navigation.Hybrid;
using UnityEngine;

namespace Ecs.Game.Children {
	public sealed class AuthoringAgentGCBehaviour : AGameSubBehaviour {
		[SerializeField] private AgentAuthoring _agent;
		
		public override void Link(GameEntity entity) => entity.AddAuthoringAgent(_agent);
	}
}