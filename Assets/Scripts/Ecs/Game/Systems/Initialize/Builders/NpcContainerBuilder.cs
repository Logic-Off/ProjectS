using Ecs.Inventory;
using Utopia;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public class NpcContainerBuilder : IAgentBuilder {
		private readonly IContainerFactory _factory;
		public NpcContainerBuilder(IContainerFactory factory) => _factory = factory;
		public bool Accept(GameEntity entity) => !entity.IsPlayer;

		public void Apply(GameEntity agent) => _factory.Create(agent.Id.Value, EContainerType.Inventory, 10);
	}
}