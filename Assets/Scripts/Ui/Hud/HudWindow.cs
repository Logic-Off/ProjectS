using Utopia;

namespace Ui.Hud {
	[InstallerGenerator(InstallerId.Ui)]
	public class HudWindow : APrebuildWindow, IMainWindow {
		public override EWindowName Name => EWindowName.Hud;

		public override void AddPanelBuilders() {
			AddBuilder<HudBuilder>();
			AddBuilder<StatusBuilder>();
			AddBuilder<MovementJoystickBuilder>();
			AddBuilder<ActionButtonsBuilder>();
			
#if DEBUG
			AddBuilder<Cheats.CheatsHudBuilder>();
#endif
		}
	}
}