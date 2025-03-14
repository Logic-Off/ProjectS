using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Ecs.Save;
using Utopia;
using Zenject;

namespace Ecs.Common {
	[InstallerGenerator(InstallerId.Game, 75_000)]
	public class InitializeSceneSystem : IInitializable {
		private readonly List<IOnSceneLoadedListener> _sceneLoadedListeners;
		private readonly List<IOnScenePostLoadedListener> _scenePostLoadedListeners;
		private readonly List<IOnFirstLoadedListener> _firstLoadedListeners;
		private readonly List<ILocationLoadProcessor> _locationLoadProcessors;

		public InitializeSceneSystem(
			List<IOnSceneLoadedListener> sceneLoadedListeners,
			List<IOnFirstLoadedListener> firstLoadedListeners,
			List<ILocationLoadProcessor> locationLoadProcessors,
			List<IOnScenePostLoadedListener> scenePostLoadedListeners
		) {
			_sceneLoadedListeners = sceneLoadedListeners;
			_firstLoadedListeners = firstLoadedListeners;
			_locationLoadProcessors = locationLoadProcessors;
			_scenePostLoadedListeners = scenePostLoadedListeners;
		}

		public void Initialize() => OnSceneLoad();

		private async UniTaskVoid OnSceneLoad() {
			await UniTask.DelayFrame(10);
			for (var index = 0; index < _sceneLoadedListeners.Count; index++) {
				var listener = _sceneLoadedListeners[index];
				listener.OnSceneLoaded();
			}

			await UniTask.DelayFrame(3);

			foreach (var listener in _scenePostLoadedListeners)
				listener.OnScenePostLoaded();

			await UniTask.DelayFrame(3);

			LocationSave save = null;
			if (save == null) {
				foreach (var listener in _firstLoadedListeners)
					listener.OnFirstLoaded();
				// Включить сохранение
				return;
			}

			foreach (var loadProcessor in _locationLoadProcessors) {
				loadProcessor.Load(save);
			}
			// Включить сохранение
		}
	}
}