using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Ecs.Ability {
	[Serializable]
	public struct AbilityData {
		[BoxGroup("Data")]
		public AbilityId Id;
		[BoxGroup("Data")]
		public EAbilityType Type;
		[BoxGroup("Data")]
		public float Cooldown;
		[BoxGroup("Data")]
		public bool IsLookAtTarget;
		[BoxGroup("Data")]
		public bool IsStandingCast;
		[BoxGroup("Data")]
		public string Animation;
		[BoxGroup("Data")]
		public string AbilityState;

		[BoxGroup("Parameters")]
		public List<AbilityParameterData> Parameters;
	}
}