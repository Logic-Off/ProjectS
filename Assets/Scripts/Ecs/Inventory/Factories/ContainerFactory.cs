using System.Collections.Generic;
using Ecs.Common;
using Ecs.Item;
using Utopia;

namespace Ecs.Inventory {
	[InstallerGenerator(InstallerId.Game)]
	public sealed class ContainerFactory : IContainerFactory {
		private readonly InventoryContext _storekeeper;
		private readonly IContainerSettingsDatabase _containerSettingsDatabase;
		private readonly ChangeCellsController _changeCellsController;
		private readonly IContainerFiltersDatabase _containerFilters;
		private readonly ContainerIdGenerator _idGenerator;

		public ContainerFactory(
			InventoryContext storekeeper,
			IContainerSettingsDatabase containerSettingsDatabase,
			ChangeCellsController changeCellsController,
			IContainerFiltersDatabase containerFilters,
			ContainerIdGenerator idGenerator
		) {
			_storekeeper = storekeeper;
			_containerSettingsDatabase = containerSettingsDatabase;
			_changeCellsController = changeCellsController;
			_containerFilters = containerFilters;
			_idGenerator = idGenerator;
		}

		public InventoryEntity Create(Id owner, EContainerType type, int size) {
			var container = CreateEntity(owner, type);

			container.AddCellSettings(new Dictionary<ECellType, int>() { { ECellType.Cell, size } });
			_changeCellsController.OnChangeCellSettings(container);

			return container;
		}

		public InventoryEntity Create(Id owner, EContainerType type, string cellSettingsId) {
			var container = CreateEntity(owner, type);
			var containerSettings = _containerSettingsDatabase.Get(cellSettingsId);
			var cellSettings = new Dictionary<ECellType, int>();
			foreach (var entry in containerSettings.Cells)
				cellSettings.Add(entry.Type, entry.Quantity);

			container.ReplaceCellSettings(new Dictionary<ECellType, int>(cellSettings));
			_changeCellsController.OnChangeCellSettings(container);

			return container;
		}

		private InventoryEntity CreateEntity(Id owner, EContainerType type) {
			var container = _storekeeper.CreateEntity();
			container.AddContainerId(_idGenerator.Next());
			container.AddContainerType(type);
			container.AddOwner(owner);
			container.AddCells(new List<CellId>());
			container.AddCellsPool(new List<CellId>());
			container.AddItemTypes(GetItemTypes(type));

			container.IsChanged = true;
			return container;
		}

		public List<EItemType> GetItemTypes(EContainerType type)
			=> new List<EItemType>(_containerFilters.Get(type).ItemTypes);
	}
}