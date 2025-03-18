using System;
using Common;
using Utopia;

namespace Ui.PlayerSkills {
	[InstallerGenerator(InstallerId.Ui)]
	public class PlayerSkillsPresenter : IDisposable {
		public readonly EventProperty<bool> IsVisible = new();

		public void Dispose() => IsVisible?.Dispose();
	}
}
