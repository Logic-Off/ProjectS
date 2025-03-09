using System;

namespace Ecs.Animations {
	[Serializable]
	public struct AnimationEventData {
		public EAnimationEvent EventName;
		public string StringValue;
		public bool BoolValue;
		public float FloatValue;
		public int IntValue;
	}
}