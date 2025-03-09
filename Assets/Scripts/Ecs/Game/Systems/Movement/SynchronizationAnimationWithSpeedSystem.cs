using UnityEngine;
using Utopia;
using Zentitas;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public class SynchronizationAnimationWithSpeedSystem : IUpdateSystem {
		private static readonly int Speed = Animator.StringToHash("Speed");
		private readonly IGroup<GameEntity> _group;
		private readonly InputContext _input;

		public SynchronizationAnimationWithSpeedSystem(GameContext game, InputContext input) {
			_input = input;
			_group = game.GetGroup(GameMatcher.AllOf(GameMatcher.Animator).AnyOf(GameMatcher.NavmeshAgent, GameMatcher.AuthoringAgent).NoneOf(GameMatcher.Dead));
		}

		public void Update() {
			foreach (var agent in _group) {
				var magnitude = GetMagnitude(agent);
				if (magnitude < 0.1f)
					agent.Animator.Value.SetFloat(Speed, 0);
				else
					agent.Animator.Value.SetFloat(Speed, magnitude);
			}
		}

		private float GetMagnitude(GameEntity agent) {
			if (agent.HasPlayer && _input.HasPlayerInput) {
				var playerInput = _input.PlayerInputEntity;
				var speed = agent.NavmeshAgent.Value.speed;
				var magnitude = playerInput.Movement.Value.magnitude * speed;
				return magnitude;
			}

			var velocity = GetVelocity(agent);
			return velocity.magnitude;
		}

		private Vector3 GetVelocity(GameEntity agent) {
			if (agent.HasAuthoringAgent) {
				var authoringAgent = agent.AuthoringAgent.Value;
				var authoringVelocity = authoringAgent.HasEntityBody ? authoringAgent.EntityBody.Velocity : authoringAgent.DefaultBody.Velocity;
				return new Vector3(authoringVelocity.x, 0, authoringVelocity.z);
			}

			var velocity = agent.NavmeshAgent.Value.velocity;
			return new Vector3(velocity.x, 0, velocity.z);
		}
	}
}