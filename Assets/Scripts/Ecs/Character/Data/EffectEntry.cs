using System;

namespace Ecs.Character {
	[Serializable]
	public struct EffectEntry {
		public ECharacterStat Stat;
		public float Count;
		public float Multiplier;
	}
}