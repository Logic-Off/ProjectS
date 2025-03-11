using Common;
using Ecs.Character;
using Ecs.Common;
using Ecs.Structures;
using Utopia;
using Zenject;
using Zentitas;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public class NpcInitializeSystem : IOnSceneLoadedListener {
		private readonly GameContext _game;
		private readonly CharacterFactory _characterFactory;
		private readonly StructureContext _structure;

		public NpcInitializeSystem(GameContext game, CharacterFactory characterFactory, StructureContext structure) {
			_game = game;
			_characterFactory = characterFactory;
			_structure = structure;
		}

		public void OnSceneLoaded() {
			var list = ListPool<StructureEntity>.Get();
			list.AddRange(_structure.GetEntitiesWithSpawnPoint(ESpawnPointType.Npc));
			for (int i = 0; i < 30; i++) {
				var point = list.Random();
				var agentEntity = _game.CreateEntity();
				agentEntity.AddId(IdGenerator.GetNext());
				agentEntity.AddPrefab("Zombie");
				agentEntity.AddPosition(point.Position.Value);
				agentEntity.AddRotation(point.Rotation.Value);
				agentEntity.IsNpc = true;
				if (point.HasWaypoints) {
					agentEntity.AddWaypoints(point.Waypoints.Values);
					agentEntity.AddWaypointIndex(0);
				}

				_characterFactory.Create(agentEntity, "Zombie", 1);
			}
		}
	}
}