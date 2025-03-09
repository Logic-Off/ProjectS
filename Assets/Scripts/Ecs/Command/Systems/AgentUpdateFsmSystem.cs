using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.Command {
	[InstallerGenerator(InstallerId.Game)]
	public class AgentUpdateFsmSystem : IUpdateSystem {
		private readonly List<GameEntity> _buffer = new();
		private readonly CommandFsm _fsm;
		private readonly IGroup<GameEntity> _group;

		public AgentUpdateFsmSystem(GameContext game, CommandFsm fsm) {
			_fsm = fsm;
			_group = game.GetGroup(GameMatcher.Fsm);
		}

		public void Update() {
			_group.GetEntities(_buffer);
			foreach (var entity in _buffer)
				_fsm.Update(entity);

			_buffer.Clear();
		}
	}
}