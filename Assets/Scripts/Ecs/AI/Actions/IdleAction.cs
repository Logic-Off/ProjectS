using Utopia;

namespace Ecs.AI {
	[InstallerGenerator(InstallerId.Game)]
	public class IdleAction : IAction {
		public float GetScore(GameEntity entity) => 1f;

		public void Execute(GameEntity agent) {
			D.Error("[AttackAction]", agent.Id.Value, " стоит");
		}
	}
}