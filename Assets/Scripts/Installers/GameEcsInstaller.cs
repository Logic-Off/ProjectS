namespace Installers {
	public sealed class GameEcsInstaller : AEcsInstaller {
		protected override string Name => "Game";

		public override void InstallBindings() {
			base.InstallBindings();
			GameInstaller.Install(Container);
			InventoryInstaller.Install(Container);
			UiInstaller.Install(Container);
		}

		protected override void InstallSystems(){}
	}
}