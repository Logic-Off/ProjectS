using Ecs.Shared;
using Utopia;

namespace Ecs.Save {
	[InstallerGenerator(InstallerId.Inventory)]
	public class InventorySaveProcessor : ASaveProcessor {
		public const string SaveKey = "Inventory";
		public override int Order => 10000;
		
		private readonly SharedContext _shared;
		private readonly IDataService _dataService;

		public InventorySaveProcessor(SharedGlobalContext shared, IDataService dataService) {
			_shared = shared;
			_dataService = dataService;
		}

		public override void Save() => _dataService.SetObject(SaveKey, _shared.Inventory.Values);
	}
}