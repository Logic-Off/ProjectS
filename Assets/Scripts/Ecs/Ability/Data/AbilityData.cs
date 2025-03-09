using System;
using System.Collections.Generic;

namespace Ecs.Ability {
	[Serializable]
	public struct AbilityData {
		public AbilityId Id;
		public EAbilityType Type;
		public float Cooldown;
		public bool IsLookAtTarget;
		public bool IsStandingCast;
		public string Animation;

		public List<AbilityParameterData> Parameters;
	}
}