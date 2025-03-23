using Ecs.Common;

namespace Ecs.Ability {
	public interface IAbilityFactory {
		AbilityEntity Create(AbilityId id, Id owner);
	}
}