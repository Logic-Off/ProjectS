using Utopia;

namespace Ui.Hud {
	[InstallerGenerator(InstallerId.Ui)]
	public class ActionButtonsInteractor {
		private readonly InputContext _input;

		public ActionButtonsInteractor(InputContext input) => _input = input;

		public void CastDefaultAbility(bool isPressed) => _input.PlayerInputEntity.IsAttackPressed = isPressed;
	}
}