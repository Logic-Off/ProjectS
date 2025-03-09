using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.Ability {
	[InstallerGenerator(InstallerId.Game)]
	public class AutoStartAbilitiesSystem : ReactiveSystem<GameEntity> {
		private readonly IAbilityFactory _abilityFactory;

		public AutoStartAbilitiesSystem(GameContext game, IAbilityFactory abilityFactory) : base(game) => _abilityFactory = abilityFactory;

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
			=> context.CreateCollector(GameMatcher.Abilities.Added());

		protected override bool Filter(GameEntity entity)
			=> entity.HasAbilities;

		protected override void Execute(List<GameEntity> entities) {
			foreach (var entity in entities) {
				foreach (var abilityId in entity.Abilities.Values)
					_abilityFactory.Create(abilityId, entity.Id.Value);
			}
		}
	}
}