using Ecs.Character;
using Ecs.Common;
using UnityEngine;
using Utopia;
using Zenject;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public class PlayerInitializeSystem : IInitializable {
		private readonly GameContext _game;
		private readonly CharacterFactory _characterFactory;
		public PlayerInitializeSystem(GameContext game, CharacterFactory characterFactory) {
			_game = game;
			_characterFactory = characterFactory;
		}

		public void Initialize() {
			var playerEntity = _game.CreateEntity();
			playerEntity.AddId(IdGenerator.GetNext());
			playerEntity.AddPrefab("Player");
			playerEntity.AddPosition(Vector3.zero);
			playerEntity.AddRotation(Quaternion.identity);
			playerEntity.IsPlayer = true;

			var player = _characterFactory.Create(playerEntity, "Player", 1);
		}
	}
}