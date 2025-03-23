using Ecs.Ui;
using Utopia;

namespace Ui.PlayerInventory {
	[InstallerPanelGenerator(InstallerId.Ui)]
	public class PlayerInventoryBuilder : APanelBuilder {
		protected override EPanelName PanelName => EPanelName.PlayerInventory;

		private readonly PlayerInventoryPresenter _presenter;
		private readonly PlayerInventoryInteractor _interactor;

		public PlayerInventoryBuilder(
			PlayerInventoryPresenter presenter,
			PlayerInventoryInteractor interactor
		) {
			_presenter = presenter;
			_interactor = interactor;
		}

		protected override void BindView(UiEntity entity) {

			_presenter.Containers.Value[EUiContainerType.Common] = Find("InventoryCellsContainer").Rect.Value;
			_presenter.Containers.Value[EUiContainerType.Equipment] = Find("EquipmentContainer").Rect.Value;
			// _presenter.Containers.Value[EUiContainerType.Spine] = Find("SpineCellContainer").RectTransform.Value;
			// _presenter.Containers.Value[EUiContainerType.Belt] = Find("BeltCellContainer").RectTransform.Value;
			_presenter.PanelId.Value = entity.Id.Value;

			_presenter.IsVisible.AddListener(x => entity.IsVisible = x);
		}

		protected override void BindInteractor() {
			_presenter.OnActivate.AddListener(_interactor.CreateCells);
			_presenter.OnShow.AddListener(_interactor.OnShow);
			_presenter.OnHide.AddListener(_interactor.OnHide);
		}

		protected override void Activate() => _presenter.OnActivate.Fire();
	}
}