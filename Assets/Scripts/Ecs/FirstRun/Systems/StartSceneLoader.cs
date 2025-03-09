using Ecs.Shared;
using Utopia;
using Zenject;

namespace Ecs {
	[InstallerGenerator(InstallerId.Project, 500_000, EInstallType.NonLazy)]
	public class StartSceneLoader : IInitializable {
		private readonly SharedContext _shared;
		
		public StartSceneLoader(SharedContext shared) => _shared = shared;

		public void Initialize() {
			var sceneEntity = _shared.CreateEntity();
			sceneEntity.IsScene = true;
			OnCreate(sceneEntity);
		}

		private void OnCreate(SharedEntity sceneEntity) {
#if UNITY_EDITOR
			var location = new LocationId(UnityEditor.EditorPrefs.GetString("Editor.FirstScene", LocationId.Base));
#else
			var location = LocationId.Base;
#endif
			sceneEntity.AddCurrentLocation(location);
			sceneEntity.AddLoadLocation(location);
		}
	}
}