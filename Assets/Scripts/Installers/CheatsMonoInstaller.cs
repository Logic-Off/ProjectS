using UnityEngine;
using Zenject;

namespace Installers {
	public sealed class CheatsMonoInstaller : MonoInstaller {
		[SerializeField] private ScriptableObjectInstaller[] _prefabsInstallers;

		public override void InstallBindings() {
#if DEBUG
			CheatsInstaller.Install(Container);
			foreach (var installer in _prefabsInstallers) {
				Container.Inject(installer);
				installer.InstallBindings();
			}
#endif
		}
	}
}