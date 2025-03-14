using Ecs.Shared;
using Utopia;

namespace Ecs.Save {
	[InstallerGenerator(InstallerId.Inventory)]
	public class PlayerInventorySaveProcessor : ASaveProcessor {
		public const string SaveKey = "PlayerInventory";
		public override int Order => 15;

		private readonly SharedContext _shared;
		private readonly IDataService _dataService;

		public PlayerInventorySaveProcessor(SharedGlobalContext shared, IDataService dataService) {
			_shared = shared;
			_dataService = dataService;
		}

		public override void Save() => _dataService.SetObject(SaveKey, _shared.PlayerInventory.Values);
	}
}