using UnityEngine;
using UnityEngine.InputSystem;
using Utopia;
using Zenject;

namespace Ecs.Input {
	[InstallerGenerator("Game")]
	public class PlayerInputSystem : IInitializable {
		private readonly InputContext _input;

		public PlayerInputSystem(InputContext input) {
			_input = input;
		}

		public void Initialize() {
			var actions = InputSystem.actions;
			actions["Move"].performed += OnMove;
			actions["Move"].canceled += OnCancel;
		}

		private void OnMove(InputAction.CallbackContext context) {
			var playerInput = _input.PlayerInputEntity;
			playerInput.ReplaceMovement(context.ReadValue<Vector2>());
		}

		private void OnCancel(InputAction.CallbackContext context) {
			var playerInput = _input.PlayerInputEntity;
			playerInput.ReplaceMovement(Vector2.zero);
		}
	}
}