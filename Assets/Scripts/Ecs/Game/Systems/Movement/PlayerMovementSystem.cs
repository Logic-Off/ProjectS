using UnityEngine;
using Utopia;
using Zentitas;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public class PlayerMovementSystem : IUpdateSystem {
		private readonly GameContext _game;
		private readonly InputContext _input;

		public PlayerMovementSystem(GameContext game, InputContext input) {
			_game = game;
			_input = input;
		}

		public void Update() {
			if (!_game.HasCamera || !_game.HasPlayer)
				return;
			
			var player = _game.PlayerEntity;
			if (!player.HasNavmeshAgent || player.IsDead)
				return;
			
			var inputMovement = _input.PlayerInputEntity.Movement.Value;
			if (inputMovement == Vector2.zero)
				return;
			
			var movement = new Vector3(inputMovement.x, 0, inputMovement.y);
			OnMove(player, movement);
			OnRotate(player, movement);
		}

		private void OnRotate(GameEntity player, Vector3 movement) {
			var toRotation = Quaternion.LookRotation(movement, Vector3.up);
			var transform = player.Transform.Value;
			transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 30);
		}

		private void OnMove(GameEntity player, Vector3 movement) {
			var agent = player.NavmeshAgent.Value;
			var translation = movement * agent.speed * Time.deltaTime;
			agent.Move(translation);
		}
	}
}