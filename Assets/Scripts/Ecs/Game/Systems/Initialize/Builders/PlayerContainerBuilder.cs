using Ecs.Inventory;
using Utopia;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public class PlayerContainerBuilder : IAgentBuilder {
		private readonly IContainerFactory _factory;
		public PlayerContainerBuilder(IContainerFactory factory) => _factory = factory;
		public bool Accept(GameEntity entity) => entity.IsPlayer;

		public void Apply(GameEntity agent) {
			_factory.Create(agent.Id.Value, EContainerType.Inventory, 10);
			_factory.Create(agent.Id.Value, EContainerType.Weapon, 1).IsActiveContainer = true;
			_factory.Create(agent.Id.Value, EContainerType.Helm, 1).IsActiveContainer = true;
			_factory.Create(agent.Id.Value, EContainerType.Torso, 1).IsActiveContainer = true;
			_factory.Create(agent.Id.Value, EContainerType.Gloves, 1).IsActiveContainer = true;
			_factory.Create(agent.Id.Value, EContainerType.Pants, 1).IsActiveContainer = true;
			_factory.Create(agent.Id.Value, EContainerType.Boots, 1).IsActiveContainer = true;
			_factory.Create(agent.Id.Value, EContainerType.Belt, 1).IsActiveContainer = true;
			_factory.Create(agent.Id.Value, EContainerType.Amulet, 1).IsActiveContainer = true;
			_factory.Create(agent.Id.Value, EContainerType.Rings, 2).IsActiveContainer = true;
		}
	}
}