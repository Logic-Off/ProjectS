using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public class ReturnToProjectilePoolSystem : ReactiveSystem<GameEntity> {
		private readonly ProjectilePool _pool;
		public ReturnToProjectilePoolSystem(GameContext game, ProjectilePool pool) : base(game) => _pool = pool;

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
			=> context.CreateCollector(GameMatcher.ReturnedToPool.Added());

		protected override bool Filter(GameEntity entity)
			=> entity.IsReturnedToPool && entity.GameType.Value is EGameType.Projectile;

		protected override void Execute(List<GameEntity> entities) {
			foreach (var entity in entities)
				_pool.Release(entity);
		}
	}
}