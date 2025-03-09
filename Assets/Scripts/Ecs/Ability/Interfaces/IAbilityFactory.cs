using Ecs.Common;

namespace Ecs.Ability {
	public interface IAbilityFactory {
		void Create(AbilityId id, Id owner);
	}
}