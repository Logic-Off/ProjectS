using System;
using Common;
using Utopia;

namespace Ui.Hud {
	[InstallerGenerator(InstallerId.Ui)]
	public class ActionButtonsPresenter : IDisposable{
		public EventProperty<bool> OnCastDefaultAbility = new();

		public void Dispose() => OnCastDefaultAbility?.Dispose();
	}
}
