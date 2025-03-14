using Ui;
using Utopia;

namespace Ecs.Ui {
	[InstallerGenerator(InstallerId.Ui)]
	public class MultiTouchesActivator : IOnWindowChangeListener {
		public void OnWindowChange(IWindow window) => UnityEngine.Input.multiTouchEnabled = window.Name == EWindowName.Hud;
	}
}