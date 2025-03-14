using System.Threading.Tasks;
using Ecs.Common;
using Ecs.Inventory;
using UnityEngine;

namespace Ui.PlayerInventory {
	public interface ILockedCellPool {
		Task<UiEntity> Get(Id parentId, ContainerId containerId, int index, Transform container, EUiCellPrefabType type);
		void Return(UiEntity entity);
	}
}