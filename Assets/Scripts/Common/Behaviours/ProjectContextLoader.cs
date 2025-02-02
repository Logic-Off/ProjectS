using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Common {
	public sealed class ProjectContextLoader : MonoBehaviour {
		[SerializeField] private AssetReference _projectContext;

		public async void Awake() {
			var handle = _projectContext.LoadAssetAsync<GameObject>();
			await handle.Task;
			ProjectContext.CurrentPrefab = handle.Result;
			var _ = ProjectContext.Instance; // Инициализация контекста
		}
	}
}