using System;
using System.Collections.Generic;

namespace Ecs.Animations {
	[Serializable]
	public struct AnimationData {
		public string Id;
		public string AnimationName;
		public float Time;
		public List<AnimationEvent> Events;
#if UNITY_EDITOR
		[UnityEngine.SerializeField] private UnityEngine.AnimationClip _clip;
#endif
	}
}