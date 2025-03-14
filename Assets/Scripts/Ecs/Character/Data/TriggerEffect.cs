using System;

namespace Ecs.Character {
	[Serializable]
	public struct TriggerEffect {
		public ETriggerEffect Type;
		public bool Value;
	}
}