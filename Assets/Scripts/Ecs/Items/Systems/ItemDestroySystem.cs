using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.Item {
	/// <summary>
	/// Система уничтожающая сущность итема
	/// </summary>
	[InstallerGenerator(InstallerId.Game, 1_000_000)]
	public class ItemDestroySystem : ReactiveSystem<ItemEntity> {
		public ItemDestroySystem(ItemContext item) : base(item) { }

		protected override ICollector<ItemEntity> GetTrigger(IContext<ItemEntity> context)
			=> context.CreateCollector(ItemMatcher.Destroyed.Added());

		protected override bool Filter(ItemEntity entity)
			=> entity.IsDestroyed;

		protected override void Execute(List<ItemEntity> entities) {
			foreach (var entity in entities)
				entity.Destroy();
		}
	}
}