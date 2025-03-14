using System;
using System.Collections.Generic;
using Ecs.Common;
using Ecs.Inventory;
using UnityEngine;

namespace Ui.PlayerInventory {
	public interface ICellsFactoryFacade {
		void CreateCells(
			InventoryEntity container,
			Transform containerTransform,
			Id parentId,
			EUiCellPrefabType containerType,
			out Dictionary<CellId, Id> cells
		);

		List<Id> CreateLockedCells(
			InventoryEntity container,
			Transform containerTransform,
			Id parentId,
			EUiCellPrefabType containerType,
			Action<UiEntity> callback
		);

		void ReturnCell(UiEntity entity);
		void ReturnLockedCell(UiEntity entity);
	}
}