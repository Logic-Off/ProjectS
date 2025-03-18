using UnityEngine;

namespace Ecs.Game {
	public sealed class RestartParticleByVisibleGSubBehaviour : AGameSubBehaviour {
		[SerializeField] private ParticleSystem _particleSystem;

		public override void Link(GameEntity entity) {
			entity.SubscribeOnVisibleChange(OnChangeVisible);
		}

		private void OnChangeVisible(GameEntity entity) {
			if (entity.IsVisible)
				_particleSystem.Play(true);
		}
	}
}