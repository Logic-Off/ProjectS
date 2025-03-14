using System.Threading.Tasks;
using Ecs.Common;
using Ecs.Inventory;
using Ecs.Ui;
using UnityEngine;
using Utopia;

namespace Ui.PlayerInventory {
	[InstallerGenerator(InstallerId.Ui)]
	public sealed class LockedCellPool : AUiPool, ILockedCellPool {
		private string _prefabName = "Cell.Inventory.Locked";
		protected override string PrefabName => _prefabName;

		public LockedCellPool(IPrefabsDatabase database, ICanvasParent parent, UiContext ui) : base(database, parent, ui) { }

		public async Task<UiEntity> Get(Id parentId, ContainerId containerId, int index, Transform container, EUiCellPrefabType type) {
			var element = await base.Get(parentId, container, $"{type.ToString()}.{index}");
			element.AddTargetContainerId(containerId);
			return element;
		}

		public override void Return(UiEntity entity) {
			entity.RemoveTargetContainerId();
			entity.OnClickedChangeEventActionListeners.Values.Clear();
			base.Return(entity);
		}
	}
}