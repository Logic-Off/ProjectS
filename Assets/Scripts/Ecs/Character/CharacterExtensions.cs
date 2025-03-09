using System;

namespace Ecs.Character {
	public static class CharacterExtensions {
		public static void AddModifier(this CharacterEntity entity, ECharacterStat stat, StatModifier value) {
			switch (stat) {
				case ECharacterStat.Health:
					entity.HealthModifier.Values.Add(value);
					entity.ReplaceHealthModifier(entity.HealthModifier.Values);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(stat), stat, null);
			}
		}
	}
}