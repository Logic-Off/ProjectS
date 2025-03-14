using Ecs.Shared;
using Utopia;
using Zentitas;

namespace Ecs.Save {
	[InstallerGenerator(InstallerId.Project, 2)]
	public class SaveController : IUpdateSystem, ISaveController {
		private int _pause;

		private readonly ISaveFacade _saveFacade;
		private readonly SharedContext _shared;

		public SaveController(ISaveFacade saveFacade, SharedGlobalContext shared) {
			_saveFacade = saveFacade;
			_shared = shared;
		}

		public void Update() {
			if (_pause <= 0 || !_shared.IsSaveEnabled)
				return;

			_pause--;

			if (_pause == 0)
				OnSave();
		}

		public void Save(bool force = false) {
			_pause = 5;

			if (!_shared.IsSaveEnabled && !force)
				return;

			if (force)
				OnSave();
		}

		private void OnSave() {
			_pause = -1;
			_saveFacade.Save();
		}
	}
}