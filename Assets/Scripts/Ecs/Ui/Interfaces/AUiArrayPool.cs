using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ecs.Common;
using Ui;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Ecs.Ui {
	public abstract class AUiArrayPool : IDisposable {
		protected abstract string[] PrefabNames { get; }
		private readonly Dictionary<string, AsyncOperationHandle<GameObject>> _handles = new();
		private readonly Dictionary<string, Queue<UiEntity>> _pools = new();

		private ICanvasParent _parent;
		private UiContext _ui;

		public AUiArrayPool(IPrefabsDatabase database, ICanvasParent parent, UiContext ui) {
			_parent = parent;
			_ui = ui;

			foreach (var prefabName in PrefabNames) {
				_handles[prefabName] = database.Get(prefabName).AssetReference.LoadAssetAsync<GameObject>();
				_pools.Add(prefabName, new Queue<UiEntity>());
			}
		}

		protected async Task<UiEntity> Get(Id parentId, Transform container, string prefabName, string entityName) {
			var parentEntity = _ui.GetEntityWithId(parentId);
			var name = $"{parentEntity.Name.Value}.{entityName}";
			if (_pools[prefabName].Count > 0) {
				var element = _pools[prefabName].Dequeue();
				element.AddName(name);
				element.AddParent(parentEntity.Id.Value);
				element.Rect.Value.SetParent(container);
				// element.RectTransform.Value.name = name;
				element.IsVisible = true;
				element.IsActive = parentEntity.IsActive;
				return element;
			}

			if (!_handles[prefabName].IsDone)
				await _handles[prefabName].Task;

			var instance = Object.Instantiate(_handles[prefabName].Result, container);
			var result = instance.GetComponent<AWidget>();
			var entity = result.Build(_ui, parentEntity);
			entity.ReplaceName(name);
			entity.AddPrefab(prefabName);
			// entity.RectTransform.Value.name = name;
			entity.IsVisible = true;
			entity.IsActive = parentEntity.IsActive;
			return entity;
		}

		public virtual void Return(UiEntity entity) {
			entity.IsVisible = false;
			entity.IsActive = false;
			entity.RemoveName();
			entity.RemoveParent();
			entity.Rect.Value.SetParent(_parent.DisabledCanvas.transform);
			_pools[entity.Prefab.Value].Enqueue(entity);
		}

		public void Dispose() {
			foreach (var (key, handle) in _handles)
				Addressables.Release(handle);

			_handles.Clear();
		}
	}
}