using System.Collections.Generic;
using Ecs.Inventory;
using Utopia;

namespace Ecs.Item.Builders {
	/// <summary>
	/// Билдер настройки ячеек у итема
	/// </summary>
	[InstallerGenerator(InstallerId.Game)]
	public class ItemContainerBuilder : IItemBuilder {
		private readonly IContainerSettingsDatabase _database;
		private readonly ItemContainerFactory _itemContainerFactory;

		public ItemContainerBuilder(IContainerSettingsDatabase database, ItemContainerFactory itemContainerFactory) {
			_database = database;
			_itemContainerFactory = itemContainerFactory;
		}

		public void Build(ItemEntity item) {
			if (!_database.Has(item.ItemId.Value))
				return;

			var containerSettings = _database.Get(item.ItemId.Value);
			var cellSettings = new Dictionary<ECellType, int>();
			foreach (var entry in containerSettings.Cells)
				cellSettings.Add(entry.Type, entry.Quantity);

			item.AddCellSettings(cellSettings);

			_itemContainerFactory.Create(item.Id.Value, EContainerType.ItemContainer, item.ItemId.Value, item);
		}
	}
}