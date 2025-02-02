using System.Threading.Tasks;
using Installers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Ecs.Shared {
	[Install(InstallerId.Project)]
	public class LoadSceneController : ILoadSceneController {
		private readonly IScenesDatabase _scenesDatabase;
		private readonly SharedContext _shared;

		public LoadSceneController(IScenesDatabase scenesDatabase, SharedGlobalContext shared) {
			_scenesDatabase = scenesDatabase;
			_shared = shared;
		}

		public async Task OnLoad(LocationId locationId) {
			var sceneEntity = _shared.SceneEntity;
			sceneEntity.IsLoading = true;
			if (sceneEntity.HasLocationData) {
				var locationData = sceneEntity.LocationData;
				await Addressables.UnloadSceneAsync(locationData.Manager, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects).Task;
				await Addressables.UnloadSceneAsync(locationData.Scene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects).Task;
			}

			var data = _scenesDatabase.Get(locationId);
			var sceneInstance = await Load(data.Scene);
			var managerInstance = await Load(data.Manager);
			sceneEntity.ReplaceLocationData(sceneInstance, managerInstance);

			SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneInstance.Scene.name));
			Resources.UnloadUnusedAssets();
			sceneEntity.IsLoading = false;
		}

		private async Task<SceneInstance> Load(AssetReference scene) {
			SceneInstance instance;
			while (true) {
				var handle = Addressables.LoadSceneAsync(scene, LoadSceneMode.Additive);
				instance = await handle.Task;

				if (handle.Status is not AsyncOperationStatus.Succeeded) {
					Addressables.Release(handle);
					await Task.Yield();
					continue;
				}

				break;
			}

			return instance;
		}
	}
}