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

		public void Create(AbilityId id, Id owner) {
			var entry = _database.Get(id);

			var ability = _ability.CreateEntity();
			ability.AddId(IdGenerator.GetNext());
			ability.AddAbilityId(entry.Id);
			ability.AddAbilityType(entry.Type);
			ability.AddFloat(0);
			ability.AddOwner(owner);
			ability.AddCooldownTime(entry.Cooldown);
			ability.AddAnimationId(entry.Animation);
			ability.IsStandingCast = entry.IsStandingCast;

			var parameters = new Dictionary<EAbilityParameter, float>();
			foreach (var data in entry.Parameters)
				parameters.Add(data.Name, data.Value);

			ability.AddParameters(parameters);
		}
	}
}