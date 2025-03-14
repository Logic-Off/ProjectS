namespace Ecs.Character {
	public static class CharacterExtensions {
		public static void AddModifier(this CharacterEntity entity, StatModifier value) {
			var modifiers = entity.StatModifier.Values;
			modifiers.Add(value);
			entity.ReplaceStatModifier(modifiers);
		}
	}
}