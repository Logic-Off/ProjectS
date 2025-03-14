using System;
using System.Collections.Generic;
using Zenject;

namespace Ecs.Inventory {
	public abstract class AGiveItemsProcessor : IInitializable, IDisposable, IGiveItemsListener {
		[Inject] private IGiveItemsFacade _facade;

		public abstract bool OnGive(List<IItemData> items);

		public void Initialize() => _facade.AddListener(this);

		public void Dispose() => _facade.RemoveListener(this);
	}
}