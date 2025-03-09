using Ecs.Common;
using UnityEngine;

namespace Ecs.Game {
	public struct TargetData {
		public static TargetData None = new(Id.None, 0, Vector3.zero);

		public Id Id;
		public float Distance;
		public Vector3 LastPosition;

		public TargetData(Id id, float distance, Vector3 lastPosition) {
			Id = id;
			Distance = distance;
			LastPosition = lastPosition;
		}
	}
}