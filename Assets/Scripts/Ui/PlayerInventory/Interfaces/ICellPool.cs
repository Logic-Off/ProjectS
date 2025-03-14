using System.Threading.Tasks;
using Ecs.Common;
using Ecs.Inventory;
using UnityEngine;

namespace Ui.PlayerInventory {
	public interface ICellPool {
		Task<UiEntity> Get(Id parentId, CellId cellId, Transform container);
		void Return(UiEntity entity);
	}
}