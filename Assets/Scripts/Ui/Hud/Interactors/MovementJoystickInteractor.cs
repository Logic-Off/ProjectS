using UnityEngine;
using Utopia;

namespace Ui.Hud {
	[InstallerGenerator(InstallerId.Ui)]
	public class MovementJoystickInteractor {
		private readonly InputContext _input;

		public MovementJoystickInteractor(InputContext input) {
			_input = input;
		}

		public void OnMovement(Vector2 value) => _input.PlayerInputEntity.ReplaceMovement(value);
	}
}