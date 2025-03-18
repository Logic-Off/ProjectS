using Ecs.Inventory;
using Utopia;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public class NpcContainerBuilder : IAgentBuilder {
		private readonly IContainerFactory _factory;
		public NpcContainerBuilder(IContainerFactory factory) => _factory = factory;
		public bool Accept(GameEntity command) => !command.IsPlayer;

		public void Apply(GameEntity animationEvent) => _factory.Create(animationEvent.Id.Value, EContainerType.Inventory, 10);
	}
}