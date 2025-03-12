using System.Collections.Generic;
using Common;
using Ecs.Common;
using UnityEngine.AddressableAssets;
using Utopia;
using Zentitas;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public sealed class ChangeItemsSystem : ReactiveSystem<GameEntity> {
		private readonly IPrefabsDatabase _prefabsDatabase;

		public ChangeItemsSystem(GameContext game, IPrefabsDatabase prefabsDatabase) : base(game) => _prefabsDatabase = prefabsDatabase;

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
			=> context.CreateCollector(GameMatcher.ChangeItems.AddedOrRemoved(), GameMatcher.ItemTransformPositions.Added());

		protected override bool Filter(GameEntity entity)
			=> entity.HasChangeItems && entity.HasItemTransformPositions;

		protected override void Execute(List<GameEntity> entities) {
			foreach (var entity in entities) {
				var currentItems = entity.CurrentItems.Values;
				foreach (var (position, name) in entity.ChangeItems.Values) {
					var transformPositions = entity.ItemTransformPositions.Values;
					if (!transformPositions.ContainsKey(position))
						continue;

					if (currentItems.ContainsKey(position)) {
						Addressables.ReleaseInstance(currentItems[position]);
						currentItems.Remove(position);
					}

					if (name.IsNullOrEmpty())
						continue;

					var reference = _prefabsDatabase.Get(name).AssetReference;
					var positionTransform = transformPositions[position];
					var handle = reference.InstantiateAsync(positionTransform);
					currentItems[position] = handle;
				}

				entity.ChangeItems.Values.Clear();
				entity.ReplaceCurrentItems(currentItems);
			}
		}
	}
}