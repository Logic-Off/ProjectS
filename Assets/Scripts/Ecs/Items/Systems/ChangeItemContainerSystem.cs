using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.Item {
	[InstallerGenerator(InstallerId.Game)]
	public class ChangeItemContainerSystem : ReactiveSystem<ItemEntity> {
		private readonly InventoryContext _inventory;

		public ChangeItemContainerSystem(ItemContext items, InventoryContext inventory) : base(items)
			=> _inventory = inventory;

		protected override ICollector<ItemEntity> GetTrigger(IContext<ItemEntity> context)
			=> context.CreateCollector(ItemMatcher.CellId);

		protected override bool Filter(ItemEntity entity)
			=> entity.HasCellId;

		protected override void Execute(List<ItemEntity> entities) {
			foreach (var entity in entities) {
				var cell = _inventory.GetEntityWithCellId(entity.CellId.Value);
				entity.ReplaceContainerOwner(cell.ContainerOwner.Value);
			}
		}
	}
}