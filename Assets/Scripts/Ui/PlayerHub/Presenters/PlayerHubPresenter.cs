using System;
using Common;
using Utopia;

namespace Ui.PlayerHub {
	[InstallerGenerator(InstallerId.Ui)]
	public class PlayerHubPresenter : IDisposable{
		public Signal<EPlayerHubPanel> ShowPanel = new();
		public readonly Signal OnClose = new();

		public void Dispose() {
			ShowPanel?.Dispose();
			OnClose?.Dispose();
		}
	}
}
