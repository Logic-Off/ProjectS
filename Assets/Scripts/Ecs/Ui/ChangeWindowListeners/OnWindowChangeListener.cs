using Ui;
using Utopia;

namespace Ecs.Ui {
	[InstallerGenerator(InstallerId.Ui)]
	public class OnWindowChangeListener : IOnWindowChangeListener {
		private readonly UiContext _ui;

		public OnWindowChangeListener(UiContext ui) => _ui = ui;

		public void OnWindowChange(IWindow window) {
			var windows = _ui.GetEntitiesWithUiType(EUiType.Window);
			foreach (var entity in windows)
				entity.IsActive = false;

			var currentWindow = _ui.GetEntityWithWindowName(window.Name);
			currentWindow.IsActive = true;
		}
	}
}