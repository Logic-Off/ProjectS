using Common;
using Utopia;

namespace Ecs.Ability {
	public interface IAbilityCommand : IStrategyCommand<AbilityEntity> { }

	[InstallerGenerator(InstallerId.Game)]
	public class AbilityStrategy : AStrategy<AbilityEntity, IAbilityCommand> { }
}