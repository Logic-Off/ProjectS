namespace Ecs.AI {
	public interface IAction {
		EAiAction Name { get; }
		float GetScore(GameEntity entity);
		void Enter(GameEntity agent);
		void Exit(GameEntity agent);
		void Execute(GameEntity agent);
	}
}