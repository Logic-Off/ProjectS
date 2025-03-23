using System;
using Common;
using Utopia;

namespace Ui.Cheats {
	[InstallerGenerator(InstallerId.Cheats)]
	public class CheatsHudPresenter : IDisposable{
		public readonly Signal OnActivate = new();
		public readonly Signal OnOpenCheats = new();

		public void Dispose() {
			OnActivate?.Dispose();
			OnOpenCheats?.Dispose();
		}
	}
}