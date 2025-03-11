using Utopia;

namespace Ecs.AI {
	[InstallerGenerator(InstallerId.Game)]
	public class IdleAction : IAction {
		public EAiAction Name => EAiAction.Idle;
		public float GetScore(GameEntity entity) => entity.HasPause ? 1000f : 1f;
		public void Enter(GameEntity agent) { }

		public void Exit(GameEntity agent) { }

		public void Execute(GameEntity agent) {
			D.Error("[AttackAction]", agent.Id.Value, " стоит");
		}
	}
}