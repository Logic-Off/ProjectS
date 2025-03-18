using System;
using Common;
using Utopia;

namespace Ui.Craft {
	[InstallerGenerator(InstallerId.Ui)]
	public class CraftPresenter : IDisposable {
		public readonly EventProperty<bool> IsVisible = new();

		public void Dispose() => IsVisible?.Dispose();
	}
}