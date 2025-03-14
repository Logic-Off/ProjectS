using System.Collections.Generic;
using Ecs.Inventory;
using Ecs.Items;
using Utopia;
using Zentitas;

namespace Ecs.Item {
	[InstallerGenerator(InstallerId.Game)]
	public class ItemBuffsChangeSystem : ReactiveSystem<ItemEntity> {
		private readonly Dictionary<bool, List<ItemEntity>> _changedItems = new() {
			{ true, new List<ItemEntity>() },
			{ false, new List<ItemEntity>() },
		};

		private readonly InventoryContext _inventory;
		private readonly ActivateItemController _activateItemController;

		public ItemBuffsChangeSystem(
			ItemContext items,
			InventoryContext inventory,
			ActivateItemController activateItemController
		) : base(items) {
			_inventory = inventory;
			_activateItemController = activateItemController;
		}

		protected override ICollector<ItemEntity> GetTrigger(IContext<ItemEntity> context)
			=> context.CreateCollector(ItemMatcher.ContainerOwner.AddedOrRemoved());

		protected override bool Filter(ItemEntity entity)
			=> true;

		protected override void Execute(List<ItemEntity> entities) {
			foreach (var entity in entities)
				OnCollectItems(entity);

			OnChangeBuffs();

			_changedItems[false].Clear();
			_changedItems[true].Clear();
		}

		private void OnCollectItems(ItemEntity entity) {
			if (entity.HasContainerOwner) {
				var container = _inventory.GetEntityWithContainerId(entity.ContainerOwner.Value);
				_changedItems[container.IsActiveContainer].Add(entity);
			} else
				_changedItems[false].Add(entity);
		}

		private void OnChangeBuffs() {
			foreach (var entity in _changedItems[false])
				_activateItemController.DeactivateItem(entity);

			foreach (var entity in _changedItems[true]) {
				var container = _inventory.GetEntityWithContainerId(entity.ContainerOwner.Value);
				if (container.ContainerType.Value == EContainerType.ItemContainer)
					continue;
				_activateItemController.ActivateItem(entity, container.Owner.Value);
			}
		}
	}
}