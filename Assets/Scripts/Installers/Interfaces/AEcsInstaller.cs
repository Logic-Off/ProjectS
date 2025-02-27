// using Common;
// using Zenject;
//
// namespace Installers {
// 	public abstract class AEcsInstaller : MonoInstaller {
// 		protected abstract string Name { get; }
//
// 		public override void InstallBindings() {
// 			InstallContext(Container);
// 			InstallSystems();
//
// 			Container.BindInterfacesTo<FeatureController>().AsSingle().WithArguments(Name).NonLazy();
// 		}
//
// 		protected virtual void InstallContext(DiContainer container) {
// 			container.BindInterfacesAndSelfTo<GameContext>().AsSingle();
// 			container.BindInterfacesAndSelfTo<InventoryContext>().AsSingle();
// 			container.BindInterfacesAndSelfTo<StructureContext>().AsSingle();
// 			container.BindInterfacesAndSelfTo<SharedContext>().AsSingle();
//
// 			if (Name.IsNotNullOrEmpty())
// 				container.BindInterfacesAndSelfTo<Contexts>().AsSingle().WithArguments(Name).NonLazy();
// 		}
//
// 		protected abstract void InstallSystems();
// 	}
// }