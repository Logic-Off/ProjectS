using System.Diagnostics;
using Common;
using UnityEngine;
using Utopia;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public class PlayerVisibleHostileTargetSystem : IGizmosSystem {
		private readonly GameContext _game;
		private readonly CharacterContext _character;

		public PlayerVisibleHostileTargetSystem(GameContext game, CharacterContext character) {
			_game = game;
			_character = character;
		}

		public void OnDrawGizmos() {
			if (!_game.IsPlayer)
				return;
			var player = _game.PlayerEntity;
			if (!player.HasTransform)
				return;

			Gizmos.color = Color.yellow;
			var origin = player.Position.Value;
			var forward = player.Transform.Value.forward;
			var character = _character.GetEntityWithId(player.Id.Value);
			Gizmos.DrawLine(origin, origin + forward * character.VisionRange.Value);

			var halfAngle = character.VisionAngle.Value * 0.5f;
			var leftEdge = Quaternion.AngleAxis(-halfAngle, Vector3.up) * forward;
			var rightEdge = Quaternion.AngleAxis(halfAngle, Vector3.up) * forward;

			Gizmos.DrawLine(origin, origin + leftEdge * character.VisionRange.Value);
			Gizmos.DrawLine(origin, origin + rightEdge * character.VisionRange.Value);

#if UNITY_EDITOR
			Handles.color = Color.yellow;
			Handles.DrawWireArc(origin, Vector3.up, -player.Transform.Value.right, character.VisionAngle.Value, character.VisionRange.Value);
#endif
		}
	}
}