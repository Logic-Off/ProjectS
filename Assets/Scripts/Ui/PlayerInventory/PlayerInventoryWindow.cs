using Ui.Draggable;
using Utopia;

namespace Ui.PlayerInventory {
	[InstallerGenerator(InstallerId.Ui)]
	public class PlayerInventoryWindow : APrebuildWindow {
		public override EWindowName Name => EWindowName.PlayerInventory;

		public override void AddPanelBuilders() {
			AddBuilder<PlayerInventoryBuilder>();
			AddBuilder<DraggableBuilder>();
		}
	}
}