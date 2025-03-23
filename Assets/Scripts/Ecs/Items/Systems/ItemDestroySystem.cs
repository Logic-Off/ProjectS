using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.Item {
	/// <summary>
	/// Система уничтожающая сущность итема
	/// </summary>
	[InstallerGenerator(InstallerId.Game, 1_000_000)]
	public class ItemDestroySystem : ReactiveSystem<ItemEntity> {
		private readonly AbilityContext _ability;
		public ItemDestroySystem(ItemContext item, AbilityContext ability) : base(item) {
			_ability = ability;
		}

		protected override ICollector<ItemEntity> GetTrigger(IContext<ItemEntity> context)
			=> context.CreateCollector(ItemMatcher.Destroyed.Added());

		protected override bool Filter(ItemEntity entity)
			=> entity.IsDestroyed;

		protected override void Execute(List<ItemEntity> entities) {
			foreach (var entity in entities) {
				var abilities = _ability.GetEntitiesWithOwner(entity.Id.Value);
				foreach (var abilityEntity in abilities)
					abilityEntity.IsDestroyed = true;
				entity.Destroy();
			}
		}
	}
}