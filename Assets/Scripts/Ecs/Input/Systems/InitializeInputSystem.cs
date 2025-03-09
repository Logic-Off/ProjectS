using UnityEngine;
using Utopia;
using Zenject;

namespace Ecs.Input {
	[InstallerGenerator(InstallerId.Game)]
	public class InitializeInputSystem : IInitializable {
		private readonly InputContext _input;

		public InitializeInputSystem(InputContext input) {
			_input = input;
		}

		public void Initialize() {
			var inputEntity = _input.CreateEntity();
			inputEntity.IsPlayerInput = true;
			inputEntity.AddMovement(Vector2.zero);
			inputEntity.AddRotate(Vector2.zero);
		}
	}
}