using System;
using System.Collections.Generic;
using Zentitas;
using Zenject;

namespace Installers {
	public sealed class FeatureController : IInitializable, ITickable, IFixedTickable, ILateTickable, IDisposable {
		private readonly Feature _feature;

		public FeatureController(
			string name,
			[Inject(Source = InjectSources.Local)] List<ISystem> systems
		) {
			_feature = new Feature($"Feature[{name}]");

			foreach (var system in systems)
				_feature.Add(system);
		}

		public void Tick() {
			_feature.Update();
			_feature.Execute();
		}

		public void Initialize() {
			_feature.Activate();
			_feature.Initialize();
		}

		public void Dispose() => _feature.Deactivate();

		public void FixedTick() => _feature.FixedUpdate();

		public void LateTick() {
			_feature.LateUpdate();

			_feature.Cleanup();
		}
	}
}