using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Ecs.Animations {
	[Serializable]
	public struct AnimationData {
		[BoxGroup("Data")]
		public string Id;
		[BoxGroup("Data")]
		public string AnimationName;
		[BoxGroup("Data")]
		public float Time;
		public List<AnimationEvent> Events;
#if UNITY_EDITOR
		[BoxGroup("Data"), InfoBox("Только для редактора"), GUIColor("yellow")]
		[UnityEngine.SerializeField] private UnityEngine.AnimationClip _clip;
#endif
	}
}