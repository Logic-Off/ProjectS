using Common;
using Ecs.Common;
using Ecs.Game;
using UnityEngine;
using Utopia;

namespace Ecs.Animations {
	[InstallerGenerator(InstallerId.Game)]
	public class CreateGunHitEffectAnimationEventCommand : IAnimationEventCommand {
		public EAnimationEvent Name => EAnimationEvent.CreateGunHitEffect;
		private readonly GameContext _game;
		private readonly EffectPool _effectPool;

		public CreateGunHitEffectAnimationEventCommand(GameContext game, EffectPool effectPool) {
			_game = game;
			_effectPool = effectPool;
		}

		public bool Accept(AnimationEventEntity entity) => true;

		public void Apply(AnimationEventEntity animationEvent) {
			var owner = _game.GetEntityWithId(animationEvent.Owner.Value);
			var target = _game.GetEntityWithId(animationEvent.Target.Value);

			const string prefabKey = "PrefabName";
			var offset = Vector3.up * Random.Range(0.8f, 1.2f);
			var direction = target.Position.Value - owner.Position.Value;
			_effectPool.Get(animationEvent.String.Values[prefabKey], Id.None, (target.Position.Value + offset).RandomPositionInRadius(0.3f), Quaternion.Euler(direction), 1);
		}
	}
}