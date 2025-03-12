using System;
using UnityEngine;

namespace Ecs.Game {
	[Serializable]
	public struct WaypointEntry {
		public Vector3 Position;
		public float Pause;
		public float Radius;

		public WaypointEntry(Vector3 position, float pause = 0f, float radius = 0f) {
			Position = position;
			Pause = pause;
			Radius = radius;
		}
	}
}