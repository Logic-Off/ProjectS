using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public sealed class GameDestroySystem : ReactiveSystem<GameEntity> {
		private readonly PrefabFactory _prefabFactory;
		public GameDestroySystem(GameContext game, PrefabFactory prefabFactory) : base(game) {
			_prefabFactory = prefabFactory;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
			=> context.CreateCollector(GameMatcher.Destroyed.Added());

		protected override bool Filter(GameEntity entity)
			=> entity.HasPrefab;

		protected override void Execute(List<GameEntity> entities) {
			foreach (var entity in entities) {
				_prefabFactory.OnDestroy(entity.Id.Value);
			}
		}
	}
}