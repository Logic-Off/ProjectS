using Common;
using Ecs.Character;
using Ecs.Common;
using Ecs.Structures;
using Utopia;
using Zentitas;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public class PlayerInitializeSystem : IOnSceneLoadedListener {
		private readonly GameContext _game;
		private readonly CharacterFactory _characterFactory;
		private readonly StructureContext _structure;

		public PlayerInitializeSystem(GameContext game, CharacterFactory characterFactory, StructureContext structure) {
			_game = game;
			_characterFactory = characterFactory;
			_structure = structure;
		}

		public void OnSceneLoaded() {
			var list = ListPool<StructureEntity>.Get();
			list.AddRange(_structure.GetEntitiesWithSpawnPoint(ESpawnPointType.Player));
			var point = list.Random();
			var playerEntity = _game.CreateEntity();
			playerEntity.AddId(IdGenerator.GetNext());
			playerEntity.AddPrefab("Player");
			playerEntity.AddPosition(point.Position.Value);
			playerEntity.AddRotation(point.Rotation.Value);
			playerEntity.IsPlayer = true;

			list.ReturnToPool();

			var player = _characterFactory.Create(playerEntity, "Player", 1);

			var changeItems = playerEntity.ChangeItems.Values;
			changeItems.Add(EItemPosition.LeftHand, "Spade");
			changeItems.Add(EItemPosition.LeftHand, "Spade");
			playerEntity.ReplaceChangeItems(changeItems);
		}
	}
}