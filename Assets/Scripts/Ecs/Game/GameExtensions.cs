using UnityEngine;

namespace Ecs.Game {
	public static class GameExtensions {
		public static bool HostileTo(this GameEntity self, GameEntity other) => self.HostileTeams.Values.Contains(other.Team.Value);

		public static void StopMovement(this GameEntity entity) {
			if (entity.HasDestination)
				entity.RemoveDestination();
			entity.IsMoving = false;
			entity.AuthoringAgent.Value.Stop();
		}
		

		public static void LookAt(this GameEntity agent, GameEntity target) {
			var direction = target.Position.Value - agent.Position.Value;
			agent.ReplaceNewRotation(Quaternion.LookRotation(direction));
		}
	}
}