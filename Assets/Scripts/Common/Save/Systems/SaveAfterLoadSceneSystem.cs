using Ecs.Common;
using Utopia;

namespace Ecs.Save {
	[InstallerGenerator(InstallerId.Game)]
	public class SaveAfterLoadSceneSystem : IOnSceneLoadedListener {
		private readonly ISaveController _saveController;
		public SaveAfterLoadSceneSystem(ISaveController saveController) => _saveController = saveController;

		public void OnSceneLoaded() => _saveController.Save();
	}
}