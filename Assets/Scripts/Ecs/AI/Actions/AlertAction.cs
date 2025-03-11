using Ecs.Game;
using Utopia;

namespace Ecs.AI {
	[InstallerGenerator(InstallerId.Game)]
	public class AlertAction : IAction {
		public EAiAction Name => EAiAction.Alert;

		public float GetScore(GameEntity entity) {
			if (entity.HasHostileTarget)
				return 75f;
			return 0;
		}

		public void Enter(GameEntity agent) { }

		public void Exit(GameEntity agent) {
			agent.StopMovement();
			agent.IsDestinationReached = false;
		}

		public void Execute(GameEntity agent) {
			var target = agent.HostileTarget.Value;
			var authoringAgent = agent.AuthoringAgent.Value;

			var stoppingDistance = authoringAgent.HasEntityLocomotion ? authoringAgent.EntityLocomotion.StoppingDistance : authoringAgent.DefaultLocomotion.StoppingDistance;
			if (target.Distance > stoppingDistance + 0.2f)
				agent.ReplaceDestination(target.LastPosition);
		}
	}
}