using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.Inventory {
	[InstallerGenerator(InstallerId.Game)]
	public class CellChangeSystem : ReactiveSystem<ItemEntity> {
		private readonly InventoryContext _inventory;
		public CellChangeSystem(ItemContext item, InventoryContext inventory) : base(item) => _inventory = inventory;

		protected override ICollector<ItemEntity> GetTrigger(IContext<ItemEntity> context)
			=> context.CreateCollector(ItemMatcher.CellId.AddedOrRemoved(), ItemMatcher.Quantity.AddedOrRemoved());

		protected override bool Filter(ItemEntity entity)
			=> true;

		protected override void Execute(List<ItemEntity> entities) {
			foreach (var entity in entities) {
				var cell = _inventory.GetEntityWithCellId(entity.CellId.Value);
				cell.IsChanged = true;
			}
		}
	}
}