using System.Collections.Generic;
using Ecs.Common;
using Utopia;

namespace Ecs.Command {
	[InstallerGenerator(InstallerId.Game)]
	public sealed class CommandFsm {
		private readonly List<CommandEntity> _buffer = new();

		private readonly CommandContext _command;
		private readonly CommandComparer _commandComparer = new();
		private readonly Dictionary<Id, int> _currentStates = new();
		private readonly Dictionary<EState, ICommandState> _states = new();

		public CommandFsm(CommandContext command, List<ICommandState> states) {
			_command = command;
			foreach (var state in states)
				_states.Add(state.State, state);
		}

		public void Update(GameEntity entity) {
			if (!entity.HasCurrentCommand)
				NewCommands(entity);
			else
				UpdateCurrentCommand(entity);
			_buffer.Clear();
		}

		private void UpdateCurrentCommand(GameEntity entity) {
			var currentCommandId = entity.CurrentCommand.Value;
			var currentCommand = _command.GetEntityWithId(currentCommandId);
			var states = CollectSortStates(currentCommandId);
			var agentId = entity.Id.Value;
			if (states.Count == 0 || !_currentStates.ContainsKey(agentId) || states.Count <= _currentStates[agentId]) {
				_currentStates.Remove(agentId);
				entity.RemoveCurrentCommand();
				return;
			}

			var index = _currentStates[agentId];
			var currentStateEntity = states[index];
			var currentState = _states[currentStateEntity.StateName.Value];
			var isComplete = currentState.Execute(entity, currentStateEntity);
			if (currentCommand == null || currentCommand.IsDestroyed || currentStateEntity.State.Value is ECommandState.Failed) {
				OnRemoveCommand(entity, currentState, currentStateEntity);
				return;
			}

			if (!isComplete)
				return;

			if (currentStateEntity.State.Value is ECommandState.InProgress)
				currentStateEntity.ReplaceState(ECommandState.Success);

			if (index + 1 < states.Count && currentStateEntity.State.Value is ECommandState.Success) {
				NextState(entity, states);
				return;
			}

			currentCommand.IsDestroyed = true;
			OnRemoveCommand(entity, currentState, currentStateEntity);
		}

		private void OnRemoveCommand(GameEntity entity, ICommandState currentState, CommandEntity command) {
			currentState.Exit(entity, command);
			_currentStates.Remove(entity.Id.Value);
			entity.RemoveCurrentCommand();
		}

		private void NewCommands(GameEntity entity) {
			var (commandId, states) = GetCommandAndSortStates(entity);
			if (commandId == Id.None || states.Count == 0)
				return;

			entity.ReplaceCurrentCommand(commandId);
			NextState(entity, states);
		}

		private void NextState(GameEntity entity, List<CommandEntity> states) {
			var entityId = entity.Id.Value;
			var index = 0;

			if (_currentStates.ContainsKey(entityId)) {
				// Если есть текущий стейт, необходимо с него выйти и апнуть индекс
				var currentStateIndex = _currentStates[entityId];
				var currentState = states[currentStateIndex];
				_states[currentState.StateName.Value].Exit(entity, currentState);
				index = currentStateIndex + 1;
			}

			NewState(entity, states[index], index);
		}

		private void NewState(GameEntity entity, CommandEntity stateEntity, int index) {
			var stateName = stateEntity.StateName.Value;
			stateEntity.ReplaceState(ECommandState.InProgress);
			var state = _states[stateName];
			state.Enter(entity, stateEntity);
			state.Execute(entity, stateEntity);
			_currentStates[entity.Id.Value] = index;

			if (stateEntity.State.Value is ECommandState.Failed) {
				var currentCommand = _command.GetEntityWithId(entity.CurrentCommand.Value);
				currentCommand.IsDestroyed = true;
				OnRemoveCommand(entity, state, stateEntity);
			}
		}

		private (Id, List<CommandEntity>) GetCommandAndSortStates(GameEntity entity) {
			var rootCommands = _command.GetEntitiesWithOwner(entity.Id.Value);
			if (rootCommands.Count == 0)
				return (Id.None, null);

			foreach (var command in rootCommands) {
				if (command.IsDestroyed)
					continue;

				return (command.Id.Value, CollectSortStates(command.Id.Value));
			}

			return (Id.None, null);
		}

		private List<CommandEntity> CollectSortStates(Id commandId) {
			var commands = _command.GetEntitiesWithOwner(commandId);
			_buffer.AddRange(commands);
			_buffer.Sort(_commandComparer);
			return _buffer;
		}

		private sealed class CommandComparer : IComparer<CommandEntity> {
			public int Compare(CommandEntity a, CommandEntity b)
				// ReSharper disable twice PossibleNullReferenceException
				=> a.Id.Value.GetHashCode().CompareTo(b.Id.Value.GetHashCode());
		}
	}
}