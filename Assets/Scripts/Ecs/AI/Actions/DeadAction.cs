using Utopia;

namespace Ecs.AI {
	[InstallerGenerator(InstallerId.Game)]
	public class DeadAction : IAction {
		public EAiAction Name => EAiAction.Dead;
		public float GetScore(GameEntity entity) => entity.IsDead ? float.MaxValue : -1f;
		public void Enter(GameEntity agent) { }

		public void Exit(GameEntity agent) { }

		public void Execute(GameEntity agent) {
			D.Error("[AttackAction]", agent.Id.Value, " мертв");
		}
	}
}