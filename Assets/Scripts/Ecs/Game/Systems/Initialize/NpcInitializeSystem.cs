using Ecs.Character;
using Ecs.Common;
using UnityEngine;
using Utopia;
using Zenject;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public class NpcInitializeSystem : IInitializable {
		private readonly GameContext _game;
		private readonly CharacterFactory _characterFactory;

		public NpcInitializeSystem(GameContext game, CharacterFactory characterFactory) {
			_game = game;
			_characterFactory = characterFactory;
		}

		public void Initialize() {
			var agentEntity = _game.CreateEntity();
			agentEntity.AddId(IdGenerator.GetNext());
			agentEntity.AddPrefab("Zombie");
			agentEntity.AddPosition(new Vector3(-10, 0, 0));
			agentEntity.AddRotation(Quaternion.identity);
			agentEntity.IsNpc = true;

			_characterFactory.Create(agentEntity, "Zombie", 1);
		}
	}
}