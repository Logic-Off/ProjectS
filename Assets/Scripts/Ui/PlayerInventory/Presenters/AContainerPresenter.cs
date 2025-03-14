using System;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Ui.PlayerInventory {
	public abstract class AContainerPresenter : IDisposable {
		public readonly EventProperty<UiEntity> SelectedCell = new();

		public readonly EventProperty<Dictionary<EUiContainerType, RectTransform>> Containers = new();
		public readonly EventProperty<UnlockedCellsEntry> Cells = new(new UnlockedCellsEntry());
		public readonly EventProperty<LockedCellsEntry> LockedCells = new(new LockedCellsEntry());

		public readonly EventProperty<Dictionary<EUiContainerType, DynamicCellsEntry>> DynamicCells = new();

		public void Dispose() {
			SelectedCell?.Dispose();
			Containers?.Dispose();
			Cells?.Dispose();
			LockedCells?.Dispose();
			DynamicCells?.Dispose();

			OnDispose();
		}

		protected abstract void OnDispose();
	}
}