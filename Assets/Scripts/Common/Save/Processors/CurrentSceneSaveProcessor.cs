using Ecs.Shared;
using Utopia;

namespace Ecs.Save {
	/// <summary>
	/// Сохранение текущей сцены
	/// </summary>
	[InstallerGenerator(InstallerId.Project)]
	public class CurrentSceneSaveProcessor : ASaveProcessor {
		public const string SaveKey = "CurrentScene";
		public override int Order => 15;

		private readonly SharedContext _shared;
		private readonly IDataService _dataService;

		public CurrentSceneSaveProcessor(SharedGlobalContext shared, IDataService dataService) {
			_shared = shared;
			_dataService = dataService;
		}

		public override void Save() {
			var scene = _shared.SceneEntity;
			var save = new CurrentSceneSave();
			save.Location = scene.CurrentLocation.Value;

			_dataService.SetObject(SaveKey, save);
		}
	}
}