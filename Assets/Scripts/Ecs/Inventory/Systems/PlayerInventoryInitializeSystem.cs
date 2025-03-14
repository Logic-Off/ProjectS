using System.Collections.Generic;
using Ecs.Save;
using Ecs.Shared;
using Utopia;
using Zenject;

namespace Ecs.Inventory {
	[InstallerGenerator(InstallerId.Project)]
	public class PlayerInventoryInitializeSystem : IInitializable {
		private readonly SharedContext _shared;
		private readonly IDataService _dataService;

		public PlayerInventoryInitializeSystem(SharedGlobalContext shared, IDataService dataService) {
			_shared = shared;
			_dataService = dataService;
		}

		public void Initialize() {
			var playerInventory = _shared.CreateEntity();
			var save = _dataService.GetObject<List<ContainerSave>>(PlayerInventorySaveProcessor.SaveKey);
			playerInventory.AddPlayerInventory(save != null ? save : new List<ContainerSave>());
		}
	}
}