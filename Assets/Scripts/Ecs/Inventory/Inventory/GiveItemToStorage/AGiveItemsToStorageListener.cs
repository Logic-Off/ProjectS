using System;
using System.Collections.Generic;
using Zenject;

namespace Ecs.Inventory {
	public abstract class AGiveItemsToStorageListener : IInitializable, IDisposable, IGiveItemsToStorageListener {
		[Inject] private IGiveItemsToStorageFacade _facade;

		public abstract void OnGive(List<IItemData> items);

		public void Initialize() => _facade.AddListener(this);

		public void Dispose() => _facade.RemoveListener(this);
	}
}