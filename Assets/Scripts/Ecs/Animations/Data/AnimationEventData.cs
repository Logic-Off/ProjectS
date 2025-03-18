using System;
using System.Collections.Generic;

namespace Ecs.Animations {
	[Serializable]
	public struct AnimationEventData {
		public EAnimationEvent EventName;

		public List<ReferenceData<string, string>> StringValues;
		public List<ReferenceData<string, float>> FloatValues;
		public List<ReferenceData<string, bool>> BoolValues;
	}

	[Serializable]
	public class ReferenceData<T, V> {
		public T Key;
		public V Value;
	}
}