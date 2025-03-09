namespace Ecs.Command {
	public interface ICommandState {
		EState State { get; }
		void Enter(GameEntity agent, CommandEntity command);
		void Exit(GameEntity agent, CommandEntity command);
		bool Execute(GameEntity agent, CommandEntity command); // Change state, return true if change
	}
}