using System.Collections.Generic;
using UnityEngine;

namespace Ecs.Character {
	public sealed class CharacterBuff : ABuff {
		public override List<Effect> Effects { get; }
		public override List<TriggerEffect> Triggers { get; }
		public override EBuffType Type { get; }
		public override double EndTime { get; }

		public CharacterBuff(BuffId id, EBuffType type, List<Effect> effects, List<TriggerEffect> triggers) {
			Id = id;
			Effects = effects;
			Triggers = triggers;
			Type = type;
			EndTime = double.MaxValue;
		}

		public CharacterBuff(BuffId id, EBuffType type, List<Effect> effects, List<TriggerEffect> triggers, double duration) {
			Id = id;
			Effects = effects;
			Triggers = triggers;
			Type = type;
			EndTime = Time.realtimeSinceStartup + duration;
		}

		public CharacterBuff(BuffData data) {
			Id = data.Id;
			Effects = data.GetBuffEffects();
			Triggers = data.Triggers;
			Type = data.Type;
			EndTime = data.Duration > 0 ? Time.realtimeSinceStartup + data.Duration : double.MaxValue;
		}

		public override string ToString() => $"Buff[{Id}]";
	}
}