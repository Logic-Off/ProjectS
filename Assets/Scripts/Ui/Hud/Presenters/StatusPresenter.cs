using System;
using Common;
using Utopia;

namespace Ui.Hud {
	[InstallerGenerator(InstallerId.Ui)]
	public class StatusPresenter : IDisposable {
		public readonly EventProperty<float> HealthAmount = new();
		public readonly EventProperty<float> HungerAmount = new();
		public readonly EventProperty<float> ThirstAmount = new();
		public readonly EventProperty<float> PsycheAmount = new();
		public readonly EventProperty<float> ColdAmount = new();
		public readonly EventProperty<float> RadiationAmount = new();

		public readonly Signal OnActivate = new();

		public void Dispose() {
			HealthAmount?.Dispose();
			HungerAmount?.Dispose();
			ThirstAmount?.Dispose();
			PsycheAmount?.Dispose();
			ColdAmount?.Dispose();
			RadiationAmount?.Dispose();
			OnActivate?.Dispose();
		}
	}
}