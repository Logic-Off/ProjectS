using System;

namespace Ecs.Character {
	[Serializable]
	public struct StatModifier {
		public ECharacterStat Stat;
		public EStatModifierType Type;
		public float Value;
		public bool IsCritical;

		public StatModifier(ECharacterStat stat, EStatModifierType type, float value) {
			Stat = stat;
			Type = type;
			Value = value;
			IsCritical = false;
		}

		public StatModifier(ECharacterStat stat, EStatModifierType type, float value, bool isCritical) {
			Stat = stat;
			Type = type;
			Value = value;
			IsCritical = isCritical;
		}
	}
}