using System.Collections.Generic;
using Ecs.Game;
using UnityEngine;

namespace Ecs.Structures {
	public sealed class WaypointStructureSubBehaviour : AStructureSubBehaviour {
		[SerializeField] private List<WaypointEntry> _waypoints;
		public List<WaypointEntry> Waypoints => _waypoints;

		public override void Link(StructureEntity entity) {
			entity.AddWaypoints(_waypoints);
		}
	}
}