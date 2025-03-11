using System.Collections.Generic;
using Ecs.Game;
using UnityEngine;

namespace Ecs.Structures {
	public sealed class SpawnPointStructureSubBehaviour : AStructureSubBehaviour {
		[SerializeField] private ESpawnPointType _type;
		[SerializeField] private List<ETeam> _teams;

		public override void Link(StructureEntity entity) {
			entity.AddSpawnPoint(_type);
			entity.AddTeams(_teams);
		}
	}
}