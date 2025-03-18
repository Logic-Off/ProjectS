using System.Collections.Generic;
using Common;
using Ecs.Common;
using Ecs.Inventory;
using UnityEngine;
using Utopia;

namespace Ui.PlayerInventory {
	[InstallerGenerator(InstallerId.Ui)]
	public class PlayerInventoryPresenter : AContainerPresenter {
		public readonly EventProperty<Id> PanelId = new(Id.None);

		public readonly Signal OnActivate = new();
		public readonly Signal OnShow = new();
		public readonly Signal OnHide = new();
		public readonly EventProperty<bool> IsVisible = new();

		public PlayerInventoryPresenter() {
			Containers.Value = new Dictionary<EUiContainerType, RectTransform>();
			DynamicCells.Value = new Dictionary<EUiContainerType, DynamicCellsEntry>();

			Cells.Value.Add(EUiContainerType.Common, new Dictionary<CellId, Id>());
			Cells.Value.Add(EUiContainerType.Backpack, new Dictionary<CellId, Id>());
			Cells.Value.Add(EUiContainerType.QuickSlots, new Dictionary<CellId, Id>());

			LockedCells.Value.Add(EUiContainerType.Backpack, new List<Id>());
			LockedCells.Value.Add(EUiContainerType.QuickSlots, new List<Id>());

			var redrawCellsEvents = new Dictionary<EUiContainerType, DynamicCellsEntry> { };

			DynamicCells.Value = redrawCellsEvents;
		}

		protected override void OnDispose() {
			PanelId?.Dispose();
			OnActivate?.Dispose();
			OnShow?.Dispose();
			OnHide?.Dispose();
			IsVisible?.Dispose();
		}
	}
}