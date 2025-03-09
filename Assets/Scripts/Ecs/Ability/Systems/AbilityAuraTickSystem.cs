using Utopia;
using Zentitas;

namespace Ecs.Ability {
	[InstallerGenerator(InstallerId.Game)]
	public class AbilityAuraTickSystem : IUpdateSystem {
		private readonly AbilityContext _ability;
		private readonly AbilityStrategy _abilityStrategy;

		public AbilityAuraTickSystem(AbilityContext ability, AbilityStrategy abilityStrategy) {
			_ability = ability;
			_abilityStrategy = abilityStrategy;
		}

		public void Update() {
			var abilities = _ability.GetEntitiesWithAbilityType(EAbilityType.Aura);
			foreach (var ability in abilities)
				_abilityStrategy.Execute(ability);
		}
	}
}