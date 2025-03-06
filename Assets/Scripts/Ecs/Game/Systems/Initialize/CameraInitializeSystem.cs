using Ecs.Common;
using UnityEngine;
using Utopia;
using Zenject;

namespace Ecs.Game {
	[InstallerGenerator("Game")]
	public class CameraInitializeSystem : IInitializable {
		private readonly GameContext _game;
		public CameraInitializeSystem(GameContext game) => _game = game;

		public void Initialize() {
			var cameraEntity = _game.CreateEntity();
			cameraEntity.AddId(IdGenerator.GetNext());
			cameraEntity.AddPrefab("Camera");
			cameraEntity.AddPosition(Vector3.zero);
			cameraEntity.AddRotation(Quaternion.identity);
			cameraEntity.IsCamera = true;
		}
	}
}