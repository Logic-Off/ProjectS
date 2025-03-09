using System;

namespace Ecs.Animations {
	[Serializable]
	public struct AnimationEvent : ICloneable {
		public float Time;
		public AnimationEventData _data;

		public object Clone() => new AnimationEvent {
			Time = Time,
			_data = _data
		};
	}
}