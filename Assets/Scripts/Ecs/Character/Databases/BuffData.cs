using System;
using System.Collections.Generic;

namespace Ecs.Character {
	[Serializable]
	public struct BuffData {
		public BuffId Id;
		public EBuffType Type;
		public float Duration;
		public List<EffectEntry> Effects;
		public List<TriggerEffect> Triggers;

		public List<Effect> GetBuffEffects() {
			var list = new List<Effect>(Effects.Count);
			foreach (var entry in Effects)
				list.Add(new Effect(entry.Stat, new Parameter(entry.Count, entry.Multiplier)));
			return list;
		}
	}
}