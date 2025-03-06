using Ecs.Common;
using UnityEngine;
using Utopia;
using Zenject;

namespace Ecs.Game {
	[InstallerGenerator("Game")]
	public class PlayerInitializeSystem : IInitializable {
		private readonly GameContext _game;
		public PlayerInitializeSystem(GameContext game) => _game = game;

		public void Initialize() {
			var playerEntity = _game.CreateEntity();
			playerEntity.AddId(IdGenerator.GetNext());
			playerEntity.AddPrefab("Player");
			playerEntity.AddPosition(Vector3.zero);
			playerEntity.AddRotation(Quaternion.identity);
			playerEntity.IsPlayer = true;
		}
	}
}