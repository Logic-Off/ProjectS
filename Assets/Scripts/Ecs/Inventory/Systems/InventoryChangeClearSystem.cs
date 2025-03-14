using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.Inventory {
	[InstallerGenerator(InstallerId.Game, 100_000_000)]
	public class InventoryChangeClearSystem : ReactiveSystem<InventoryEntity> {
		public InventoryChangeClearSystem(InventoryContext inventory) : base(inventory) { }

		protected override ICollector<InventoryEntity> GetTrigger(IContext<InventoryEntity> context)
			=> context.CreateCollector(InventoryMatcher.Changed.Added());

		protected override bool Filter(InventoryEntity entity)
			=> entity.IsChanged;

		protected override void Execute(List<InventoryEntity> entities) {
			foreach (var entity in entities)
				entity.IsChanged = false;
		}
	}
}