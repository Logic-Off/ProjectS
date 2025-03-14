using System;
using Common;
using Utopia;

namespace Ui.Hud {
	[InstallerGenerator(InstallerId.Ui)]
	public class HudPresenter : IDisposable{
		public readonly Signal OnOpenInventory = new();

		public void Dispose() => OnOpenInventory?.Dispose();
	}
}
