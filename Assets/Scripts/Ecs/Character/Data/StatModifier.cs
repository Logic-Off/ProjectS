using System;

namespace Ecs.Character {
	[Serializable]
	public struct StatModifier {
		public EStatModifierType Type;
		public float Value;
		public bool IsCritical;
	}
}