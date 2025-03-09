using System;
using UnityEngine;

namespace Ecs.Game {
	[Serializable]
	public struct WaypointEntry {
		public Vector3 Position;
		public float Pause;

		public WaypointEntry(Vector3 position, float pause = 0f) {
			Position = position;
			Pause = pause;
		}
	}
}