using Ecs.Game;
using UnityEngine;
using Utopia;

namespace Ecs.AI {
	[InstallerGenerator(InstallerId.Game)]
	public class PatrolAction : IAction {
		public EAiAction Name => EAiAction.Patrol;

		public float GetScore(GameEntity entity) {
			if (!entity.HasWaypoints || entity.HasCurrentCommand)
				return 0f;

			return 30f;
		}

		public void Enter(GameEntity agent) {
			var waypoints = agent.Waypoints.Values;
			agent.ReplaceDestination(waypoints[agent.WaypointIndex.Value].Position);
			agent.IsDestinationReached = false;
		}

		public void Exit(GameEntity agent) => agent.StopMovement();

		public void Execute(GameEntity agent) {
			if (!agent.IsDestinationReached)
				return;

			var index = agent.WaypointIndex.Value;
			var currentWaypoint = agent.Waypoints.Values[index];
			if (currentWaypoint.Pause > 0.1f) {
				ChangeIndexWithPause(agent, index, currentWaypoint);
			} else
				NextWaypoint(agent);
		}

		private void NextWaypoint(GameEntity agent) {
			var index = agent.WaypointIndex.Value;
			var waypoints = agent.Waypoints.Values;

			if (index >= waypoints.Count - 1)
				agent.ReplaceWaypointIndex(0);
			else
				agent.ReplaceWaypointIndex(index + 1);

			agent.ReplaceDestination(waypoints[agent.WaypointIndex.Value].Position);
			agent.IsDestinationReached = false;
		}

		private void ChangeIndexWithPause(GameEntity agent, int index, WaypointEntry currentWaypoint) {
			var waypoints = agent.Waypoints.Values;
			if (index >= waypoints.Count - 1)
				agent.ReplaceWaypointIndex(0);
			else
				agent.ReplaceWaypointIndex(index + 1);
			agent.AddPause(Time.realtimeSinceStartup, currentWaypoint.Pause);
		}
	}
}