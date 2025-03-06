using System.Collections.Generic;
using Ecs.Common;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utopia;

namespace Ecs.Game {
	[InstallerGenerator("Game")]
	public sealed class PrefabFactory {
		private Dictionary<Id, AsyncOperationHandle> _handles = new();
		private readonly IPrefabsDatabase _prefabsDatabase;

		public PrefabFactory(IPrefabsDatabase prefabsDatabase) {
			_prefabsDatabase = prefabsDatabase;
		}

		public void OnCreate(GameEntity entity) {
			var asset = _prefabsDatabase.Get(entity.Prefab.Value).AssetReference;
			var handle = asset.InstantiateAsync(entity.Position.Value, entity.Rotation.Value);
			_handles[entity.Id.Value] = handle;
			handle.Completed += handle => OnCompleted(handle, entity);
		}

		private void OnCompleted(AsyncOperationHandle<GameObject> handle, GameEntity entity) {
			var result = handle.Result;
			var behaviour = result.GetComponent<GameBehaviour>();
			if (behaviour != null)
				behaviour.Link(entity);
		}
		
		public void OnDestroy(Id id) {
			if (_handles.ContainsKey(id)) {
				Addressables.ReleaseInstance(_handles[id]);
				_handles.Remove(id);
			}
		}
	}
}