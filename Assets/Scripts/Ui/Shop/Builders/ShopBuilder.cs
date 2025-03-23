using Utopia;

namespace Ui.Shop {
	[InstallerPanelGenerator(InstallerId.Ui)]
	public class ShopBuilder : APanelBuilder {
		protected override EPanelName PanelName => EPanelName.Shop;
		private readonly ShopPresenter _presenter;
		private readonly ShopInteractor _interactor;

		public ShopBuilder(ShopPresenter presenter, ShopInteractor interactor) {
			_presenter = presenter;
			_interactor = interactor;
		}

		protected override void BindView(UiEntity entity) {
			_presenter.IsVisible.AddListener(x => entity.IsVisible = x);
		}

		protected override void BindInteractor() {
 			base.BindInteractor();
		}

		protected override void Activate() {
			base.Activate();
		}
	}
}
