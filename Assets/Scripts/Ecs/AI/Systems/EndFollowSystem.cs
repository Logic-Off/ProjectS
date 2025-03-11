using System.Collections.Generic;
using Ecs.Game;
using Utopia;
using Zentitas;

namespace Ecs.AI {
	[InstallerGenerator(InstallerId.Game)]
	public class EndFollowSystem : ICleanupSystem {
		private readonly List<GameEntity> _buffer = new();
		private readonly IGroup<GameEntity> _group;

		public EndFollowSystem(GameContext game) => _group = game.GetGroup(GameMatcher.Moving);

		public void Cleanup() {
			_group.GetEntities(_buffer);

			foreach (var entity in _buffer) {
				var agent = entity.AuthoringAgent.Value;
				var body = agent.HasEntityBody ? agent.EntityBody : agent.DefaultBody;

				if (!body.IsStopped)
					continue;

				entity.IsDestinationReached = true;
				entity.StopMovement();
			}

			_buffer.Clear();
		}
	}
}