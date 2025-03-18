using UnityEngine;
using UnityEngine.InputSystem;
using Utopia;
using Zenject;

namespace Ecs.Input {
	[InstallerGenerator(InstallerId.Game)]
	public class PlayerInputSystem : IInitializable {
		private readonly InputContext _input;

		public PlayerInputSystem(InputContext input) => _input = input;

		public void Initialize() {
			var actions = InputSystem.actions;
			actions["Move"].performed += OnMove;
			actions["Move"].canceled += OnMoveCancel;
			actions["Attack"].started += OnStartAttack;
			actions["Attack"].canceled += OnEndAttack;
		}

		private void OnMove(InputAction.CallbackContext context) => _input.PlayerInputEntity.ReplaceMovement(context.ReadValue<Vector2>());
		private void OnMoveCancel(InputAction.CallbackContext context) => _input.PlayerInputEntity.ReplaceMovement(Vector2.zero);
		private void OnStartAttack(InputAction.CallbackContext context) => _input.PlayerInputEntity.IsAttackPressed = true;
		private void OnEndAttack(InputAction.CallbackContext context) => _input.PlayerInputEntity.IsAttackPressed = false;
	}
}