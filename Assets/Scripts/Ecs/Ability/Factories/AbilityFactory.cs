using System.Collections.Generic;
using Ecs.Common;
using Utopia;

namespace Ecs.Ability {
	[InstallerGenerator(InstallerId.Game)]
	public class AbilityFactory : IAbilityFactory {
		private readonly AbilityContext _ability;
		private readonly IAbilitiesDatabase _database;

		public AbilityFactory(IAbilitiesDatabase database, AbilityContext ability) {
			_database = database;
			_ability = ability;
		}

		public AbilityEntity Create(AbilityId id, Id owner) {
			var data = _database.Get(id);

			var ability = _ability.CreateEntity();
			ability.AddId(IdGenerator.GetNext());
			ability.AddAbilityId(data.Id);
			ability.AddAbilityType(data.Type);
			ability.AddFloat(0);
			ability.AddOwner(owner);
			ability.AddCooldownTime(data.Cooldown);
			ability.AddAnimationId(data.Animation);
			ability.IsStandingCast = data.IsStandingCast;
			ability.AddAbilityState(data.AbilityState);

			var parameters = new Dictionary<EAbilityParameter, float>();
			foreach (var parameter in data.Parameters)
				parameters.Add(parameter.Name, parameter.Value);

			ability.AddParameters(parameters);
			return ability;
		}
	}
}