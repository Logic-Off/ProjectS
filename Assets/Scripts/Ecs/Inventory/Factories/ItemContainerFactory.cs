using Ecs.Common;
using Ecs.Item;
using Utopia;

namespace Ecs.Inventory {
	[InstallerGenerator(InstallerId.Game)]
	public class ItemContainerFactory {
		private readonly IContainerFactory _containerFactory;
		private readonly IItemFiltersDatabase _itemFilters;

		public ItemContainerFactory(IContainerFactory containerFactory, IItemFiltersDatabase itemFilters) {
			_containerFactory = containerFactory;
			_itemFilters = itemFilters;
		}

		public InventoryEntity Create(Id owner, EContainerType type, string cellSettingsId, ItemEntity item) {
			var container = _containerFactory.Create(owner, type, cellSettingsId);

			if (!_itemFilters.Has(item.ItemType.Value))
				return container;

			var filter = _itemFilters.Get(item.ItemType.Value);
			var itemTypes = container.ItemTypes.Value;
			itemTypes.Clear();
			itemTypes.AddRange(filter.ItemTypes);
			container.ReplaceItemTypes(itemTypes);

			return container;
		}
	}
}