using Utopia;

namespace Ui.Cheats {
	[InstallerGenerator(InstallerId.Cheats)]
	public class CheatsWindow : AWindow, IPopUp {
		public override EWindowName Name => EWindowName.Cheats;

		public override void AddPanelBuilders() {
			AddBuilder<CheatsBuilder>();
		}
	}
}