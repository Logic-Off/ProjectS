using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization.Settings;
using Zenject;

namespace Common {
	public sealed class ProjectContextLoader : MonoBehaviour {
		[SerializeField] private AssetReference _projectContext;

		public void Awake() => Initialize();

		private async UniTaskVoid Initialize() {
			var operation = LocalizationSettings.InitializationOperation;
			await operation.ToUniTask();
			var handle = _projectContext.LoadAssetAsync<GameObject>();
			await handle.ToUniTask();
			ProjectContext.CurrentPrefab = handle.Result;
			var _ = ProjectContext.Instance;
		}
	}
}