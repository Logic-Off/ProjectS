using Ecs.Shared;
using Zenject;

namespace Installers {
	public sealed class ProjectEcsInstaller : AEcsInstaller {
		protected override string Name => "Project";

		public override void InstallBindings() {
			base.InstallBindings();
			Container.BindInterfacesAndSelfTo<SharedGlobalContext>().AsSingle();
			ProjectInstaller.Install(Container);
		}

		protected override void InstallSystems() { }
	}
}