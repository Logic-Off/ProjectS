using Common;
using Utopia;
using Zentitas;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game, 3_000_000)]
	public class EffectEndTimeSystem : IUpdateSystem {
		private readonly IGroup<GameEntity> _effects;
		private readonly IClock _clock;

		public EffectEndTimeSystem(GameContext game, IClock clock) {
			_clock = clock;
			_effects = game.GetGroup(GameMatcher.AllOf(GameMatcher.GameType, GameMatcher.EndTime).NoneOf(GameMatcher.ReturnedToPool));
		}

		public void Update() {
			var removed = ListPool<GameEntity>.Get();
			foreach (var element in _effects)
				if (element.EndTime.Value <= _clock.Time)
					removed.Add(element);

			foreach (var element in removed)
				element.IsReturnedToPool = true;

			removed.ReturnToPool();
		}
	}
}