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
	public abstract class AUiPool : IDisposable {
		protected abstract string PrefabName { get; }
		private AsyncOperationHandle<GameObject> _handle;

		private Queue<UiEntity> _pool = new();

		private ICanvasParent _parent;
		private UiContext _ui;

		public AUiPool(IPrefabsDatabase database, ICanvasParent parent, UiContext ui) {
			_parent = parent;
			_ui = ui;

			_handle = database.Get(PrefabName).AssetReference.LoadAssetAsync<GameObject>();
		}

		protected async Task<UiEntity> Get(Id parentId, Transform container, string entityName) {
			var parentEntity = _ui.GetEntityWithId(parentId);
			var name = $"{parentEntity.Name.Value}.{entityName}";
			if (_pool.Count > 0) {
				var element = _pool.Dequeue();
				element.AddName(name);
				element.AddParent(parentEntity.Id.Value);
				element.Rect.Value.SetParent(container);
				// element.RectTransform.Value.name = name;
				element.IsVisible = true;
				element.IsActive = parentEntity.IsActive;
				return element;
			}

			if (!_handle.IsDone)
				await _handle.Task;

			var instance = Object.Instantiate(_handle.Result, container);
			var result = instance.GetComponent<AWidget>();
			var entity = result.Build(_ui, parentEntity);
			entity.ReplaceName(name);
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
			_pool.Enqueue(entity);
		}

		public void Dispose() => Addressables.Release(_handle);
	}
}