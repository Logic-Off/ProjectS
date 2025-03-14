using System.Threading.Tasks;
using Ecs.Common;
using Ecs.Inventory;
using Ecs.Ui;
using UnityEngine;
using Utopia;

namespace Ui.PlayerInventory {
	[InstallerGenerator(InstallerId.Ui)]
	public sealed class CellPool : AUiPool, ICellPool {
		private string _prefabName = "Cell.Inventory.Default";
		protected override string PrefabName => _prefabName;

		public CellPool(IPrefabsDatabase database, ICanvasParent parent, UiContext ui) : base(database, parent, ui) { }

		public async Task<UiEntity> Get(Id parentId, CellId cellId, Transform container) {
			var element = await base.Get(parentId, container, cellId.ToString());
			element.AddTargetCellId(cellId);
			return element;
		}

		public override void Return(UiEntity entity) {
			entity.RemoveTargetCellId();
			entity.OnClickedChangeEventActionListeners.Values.Clear();
			entity.OnSelectedChangeEventListeners.Values.Clear();
			entity.ReplaceSelected(false);
			base.Return(entity);
		}
	}
}