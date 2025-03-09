using Ecs.Common;
using JetBrains.Annotations;

namespace Ecs.Ability {
	public static class AbilityExtensions {
		public static bool CanCastByDistance(this AbilityEntity ability, float currentDistance) {
			var parameters = ability.Parameters.Values;

			// Если текущая дистанция меньше минимальной - значит не можем кастовать абилку
			if (parameters.ContainsKey(EAbilityParameter.MinDistance) && parameters[EAbilityParameter.MinDistance] > currentDistance)
				return false;

			// Если текущая дистанция больше чем дистанция каста абилки - значит не можем кастовать абилку
			if (parameters.ContainsKey(EAbilityParameter.Distance) && parameters[EAbilityParameter.Distance] < currentDistance)
				return false;

			// Если нет дистанций или условия прерывания не соблюдены - можем кастовать абилку
			return true;
		}

		[CanBeNull]
		public static AbilityEntity GetAbility(this AbilityContext context, Id owner, AbilityId abilityId) {
			var abilities = context.GetEntitiesWithOwner(owner);
			foreach (var ability in abilities)
				if (ability.AbilityId.Value == abilityId)
					return ability;

			return null;
		}
	}
}