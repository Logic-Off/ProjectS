// using System;
// using System.Collections.Generic;
// using Ecs.Inventory;
// using Ecs.Item;
// using Ecs.Ui;
// using LogicOff;
// using UnityEngine;
//
// namespace Ui.PlayerInventory {
// 	[Install(InstallerId.Game)]
// 	public class CellsFactoryFacade : ICellsFactoryFacade {
// 		private readonly ICellPool _cellPool;
// 		private readonly ILockedCellPool _lockedCellPool;
//
// 		private readonly InventoryContext _inventory;
//
// 		// private readonly ICellProductsDatabase _cellProductsDatabase;
// 		private readonly ItemContext _items;
//
// 		public CellsFactoryFacade(
// 			ICellPool cellPool,
// 			ILockedCellPool lockedCellPool,
// 			InventoryContext inventory,
// 			// ICellProductsDatabase cellProductsDatabase,
// 			ItemContext items
// 		) {
// 			_cellPool = cellPool;
// 			_lockedCellPool = lockedCellPool;
// 			_inventory = inventory;
// 			// _cellProductsDatabase = cellProductsDatabase;
// 			_items = items;
// 		}
//
// 		public void CreateCells(
// 			InventoryEntity container,
// 			Transform containerTransform,
// 			Id parentId,
// 			EUiCellPrefabType containerType,
// 			out Dictionary<CellId, Id> cells
// 		) {
// 			cells = new Dictionary<CellId, Id>();
// 			foreach (var cellId in container.Cells.Value) {
// 				var cell = _inventory.GetEntityWithCellId(cellId);
// 				var cellEntity = _cellPool.Get(parentId, cell, containerTransform, containerType);
// 				cells.Add(cellId, cellEntity.Id);
//
// 				var item = cell.IsEmpty ? null : _items.GetEntityWithItemInstanceId(cell.CellTarget);
// 				cellEntity.UpdateUiCell(cell, item);
// 			}
// 		}
//
// 		public List<Id> CreateLockedCells(
// 			InventoryEntity container,
// 			Transform containerTransform,
// 			Id parentId,
// 			EUiCellPrefabType containerType,
// 			Action<UiEntity> callback
// 		) {
// 			var cells = new List<Id>();
// 			foreach (var (key, value) in container.CellSettings.Values) {
// 				if (key is ECellType.None or ECellType.Cell)
// 					continue;
//
// 				for (var i = 0; i < value; i++) {
// 					// var products = _cellProductsDatabase.Get(key);
// 					var cellEntity = _lockedCellPool.Get(parentId, container.ContainerId.Value, i, containerTransform, containerType);
// 					cells.Add(cellEntity.Id);
// 					cellEntity.AddClicked(callback);
// 					// cellEntity.ReplaceProductIds(new List<ProductId>(products.ProductIds));
// 				}
// 			}
//
// 			return cells;
// 		}
//
// 		public void ReturnCell(UiEntity entity) => _cellPool.Return(entity);
// 		public void ReturnLockedCell(UiEntity entity) => _lockedCellPool.Return(entity);
// 	}
// }