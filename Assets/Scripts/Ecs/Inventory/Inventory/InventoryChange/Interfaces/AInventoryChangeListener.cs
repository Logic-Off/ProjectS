using System;
using Ecs.Item;
using Zenject;

namespace Ecs.Inventory {
	public abstract class AInventoryChangeListener : IInventoryChangeListener, IInitializable, IDisposable {
		[Inject] private readonly IInventoryChangeFacade _facade;
		public abstract void OnChange(ItemId itemId, int quantity, EInventoryChangeType type);
		
		public void Initialize() => _facade.AddListener(this);

		public void Dispose() => _facade.RemoveListener(this);
	}
}