using Ecs.Shared;
using Utopia;

namespace Ecs.Save {
	[InstallerGenerator(InstallerId.Inventory)]
	public class StorageSaveProcessor : ASaveProcessor {
		public const string SaveKey = "Storage";
		public override int Order => 10000;

		private readonly SharedContext _shared;
		private readonly IDataService _dataService;

		public StorageSaveProcessor(SharedGlobalContext shared, IDataService dataService) {
			_shared = shared;
			_dataService = dataService;
		}

		public override void Save() => _dataService.SetObject(SaveKey, _shared.Storage.Values);
	}
}