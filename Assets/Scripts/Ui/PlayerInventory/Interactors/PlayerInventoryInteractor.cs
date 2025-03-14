using System.Threading.Tasks;
using Ecs.Inventory;
using Ecs.Item;
using Utopia;

namespace Ui.PlayerInventory {
	[InstallerGenerator(InstallerId.Ui)]
	public class PlayerInventoryInteractor {
		private readonly GameContext _game;
		private readonly PlayerInventoryPresenter _presenter;
		private readonly IWindowRouter _windowRouter;
		private readonly CellDrawerController _cellsDrawer;
		private readonly IPlayerInventory _playerInventory;

		public PlayerInventoryInteractor(
			GameContext game,
			PlayerInventoryPresenter presenter,
			IWindowRouter windowRouter,
			CellDrawerController cellsDrawer,
			IPlayerInventory playerInventory
		) {
			_game = game;
			_presenter = presenter;
			_windowRouter = windowRouter;
			_cellsDrawer = cellsDrawer;
			_playerInventory = playerInventory;
		}

		public async void CreateCells() {
			while (!_game.IsPlayer)
				await Task.Yield();
			var cells = _presenter.Cells.Value.Get(EUiContainerType.Common);
			var playerId = _game.PlayerEntity.Id.Value;
			_playerInventory.Add(new ItemId("Rifle_1"), 1);

			await _cellsDrawer.OnCreateCells(playerId, _presenter.PanelId.Value, cells, EContainerType.Inventory, _presenter.Containers.Value[EUiContainerType.Common]);
			await _cellsDrawer.OnCreateCells(playerId, _presenter.PanelId.Value, cells, EContainerType.Weapon, _presenter.Containers.Value[EUiContainerType.Equipment]);
			await _cellsDrawer.OnCreateCells(playerId, _presenter.PanelId.Value, cells, EContainerType.Helm, _presenter.Containers.Value[EUiContainerType.Equipment]);
			await _cellsDrawer.OnCreateCells(playerId, _presenter.PanelId.Value, cells, EContainerType.Torso, _presenter.Containers.Value[EUiContainerType.Equipment]);
			await _cellsDrawer.OnCreateCells(playerId, _presenter.PanelId.Value, cells, EContainerType.Pants, _presenter.Containers.Value[EUiContainerType.Equipment]);
			await _cellsDrawer.OnCreateCells(playerId, _presenter.PanelId.Value, cells, EContainerType.Boots, _presenter.Containers.Value[EUiContainerType.Equipment]);
			await _cellsDrawer.OnCreateCells(playerId, _presenter.PanelId.Value, cells, EContainerType.Belt, _presenter.Containers.Value[EUiContainerType.Equipment]);
			await _cellsDrawer.OnCreateCells(playerId, _presenter.PanelId.Value, cells, EContainerType.Gloves, _presenter.Containers.Value[EUiContainerType.Equipment]);
			await _cellsDrawer.OnCreateCells(playerId, _presenter.PanelId.Value, cells, EContainerType.Rings, _presenter.Containers.Value[EUiContainerType.Equipment]);
			await _cellsDrawer.OnCreateCells(playerId, _presenter.PanelId.Value, cells, EContainerType.Amulet, _presenter.Containers.Value[EUiContainerType.Equipment]);
			// _cellsDrawer.OnCreateCells(playerId, cells, EContainerType.Belt, EUiContainerType.Belt);
		}

		public void OnShow() {
			// var playerId = _game.PlayerEntity.Id.Value;
			// _cellsDrawer.OnRedrawCells(playerId, EUiContainerType.Backpack);
			// _cellsDrawer.OnRedrawCells(playerId, EUiContainerType.QuickSlots);
		}

		public void OnHide() {
			if (_presenter.SelectedCell.Value is null)
				return;
			_presenter.SelectedCell.Value.ReplaceSelected(false);
			_presenter.SelectedCell.Value = null;
		}

		public void OnClose() => _windowRouter.OnRoot();
	}
}