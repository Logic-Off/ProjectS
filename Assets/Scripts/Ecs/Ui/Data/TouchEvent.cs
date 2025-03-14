using UnityEngine;

namespace Ecs.Ui {
	public struct TouchEvent {
		public int Finger;
		public ETouchState State;
		public Vector2 Position;
		public Vector2 Delta;
		public GameObject Target;
	}
}