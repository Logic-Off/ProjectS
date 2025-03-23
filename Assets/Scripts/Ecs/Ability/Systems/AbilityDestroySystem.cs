using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.Ability {
	[InstallerGenerator(InstallerId.Game, 1_000_000)]
	public sealed class AbilityDestroySystem : ReactiveSystem<AbilityEntity> {
		public AbilityDestroySystem(AbilityContext ability) : base(ability) { }

		protected override ICollector<AbilityEntity> GetTrigger(IContext<AbilityEntity> context)
			=> context.CreateCollector(AbilityMatcher.Destroyed.Added());

		protected override bool Filter(AbilityEntity entity)
			=> entity.HasDestroyed;

		protected override void Execute(List<AbilityEntity> entities) {
			foreach (var entity in entities)
				entity.Destroy();
		}
	}
}