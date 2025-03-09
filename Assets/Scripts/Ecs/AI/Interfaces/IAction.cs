namespace Ecs.AI {
	public interface IAction {
		public float GetScore(GameEntity entity);
		public void Execute(GameEntity agent);
	}
}