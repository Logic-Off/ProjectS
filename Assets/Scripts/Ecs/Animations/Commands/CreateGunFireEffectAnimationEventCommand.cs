using Ecs.Common;
using Ecs.Game;
using Utopia;

namespace Ecs.Animations {
	[InstallerGenerator(InstallerId.Game)]
	public class CreateGunFireEffectAnimationEventCommand : IAnimationEventCommand {
		public EAnimationEvent Name => EAnimationEvent.CreateGunFireEffect;
		private readonly GameContext _game;
		private readonly EffectPool _effectPool;

		public CreateGunFireEffectAnimationEventCommand(GameContext game, EffectPool effectPool) {
			_game = game;
			_effectPool = effectPool;
		}

		public bool Accept(AnimationEventEntity entity) => true;

		public void Apply(AnimationEventEntity animationEvent) {
			var owner = _game.GetEntityWithId(animationEvent.Owner.Value);
			if (!owner.HasGunFirePosition)
				return;

			var transform = owner.GunFirePosition.Value;
			const string prefabKey = "PrefabName";
			_effectPool.Get(animationEvent.String.Values[prefabKey], Id.None, transform.position, transform.rotation, 1);
		}
	}
}