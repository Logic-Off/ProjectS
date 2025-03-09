using System;
using System.Collections.Generic;
using Common;
using Zentitas;
using Zenject;

namespace Installers {
	public sealed class FeatureController : IInitializable, ITickable, IFixedTickable, ILateTickable, IDisposable {
		private readonly Feature _feature;
		private readonly List<IGizmosSystem> _gizmos;

		public FeatureController(
			string name,
			[Inject(Source = InjectSources.Local)] List<ISystem> systems,
			[Inject(Source = InjectSources.Local)] List<IGizmosSystem> gizmos
		) {
			_gizmos = gizmos;
			_feature = new Feature($"Feature[{name}]");
#if UNITY_EDITOR
			var monoGizmos = new UnityEngine.GameObject("Gizmos").AddComponent<MonoGizmosBehaviour>();
			monoGizmos.Subscribe(GizmosTick);
#endif

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

		public void GizmosTick() {
			foreach (var gizmosSystem in _gizmos)
				gizmosSystem.OnDrawGizmos();
		}
	}
}