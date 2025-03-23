using Ecs.Ui;
using Utopia;

namespace Ui.Hud {
	[InstallerPanelGenerator(InstallerId.Ui)]
	public class HudBuilder : APanelBuilder {
		protected override EPanelName PanelName => EPanelName.Hud;
		private readonly HudPresenter _presenter;
		private readonly HudInteractor _interactor;

		public HudBuilder(HudPresenter presenter, HudInteractor interactor) {
			_presenter = presenter;
			_interactor = interactor;
		}

		protected override void BindView(UiEntity entity) {
			Find("InventoryButton").SubscribeOnClickedChange(x => _presenter.OnOpenInventory.Fire());
		}

		protected override void BindInteractor() {
			_presenter.OnOpenInventory.AddListener(_interactor.OpenInventory);
		}

		protected override void Activate() {
			base.Activate();
		}
	}
}
