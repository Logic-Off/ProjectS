using Ecs.Common;

namespace Ecs.Command {
	public static class CommandExtensions {
		public static CommandEntity Create(this CommandContext self, Id owner) {
			var command = self.CreateEntity();
			command.AddId(IdGenerator.GetNext());
			command.AddOwner(owner);
			return command;
		}

		public static CommandEntity CreateState(this CommandContext self, CommandEntity command, EState stateName) {
			var state = self.CreateEntity();
			state.AddId(IdGenerator.GetNext());
			state.AddOwner(command.Id.Value);
			state.AddStateName(stateName);
			state.AddState(ECommandState.None);
			state.IsStandingCommand = command.IsStandingCommand;
			return state;
		}
	}
}