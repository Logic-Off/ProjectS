using System;
using Common;
using Utopia;

namespace Ui.CharacterInfo {
	[InstallerGenerator(InstallerId.Ui)]
	public class CharacterInfoPresenter : IDisposable {
		public readonly EventProperty<bool> IsVisible = new();

		public void Dispose() => IsVisible?.Dispose();
	}
}
