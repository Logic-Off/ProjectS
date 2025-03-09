using System.Collections.Generic;
using UnityEngine;
using Utopia;
using Zentitas;

namespace Ecs.Character {
	[InstallerGenerator(InstallerId.Game, 1_000_000)]
	public class CharacterHealthModifierSystem : ReactiveSystem<CharacterEntity> {
		public CharacterHealthModifierSystem(CharacterContext character) : base(character) { }

		protected override ICollector<CharacterEntity> GetTrigger(IContext<CharacterEntity> context)
			=> context.CreateCollector(CharacterMatcher.HealthModifier.Added());

		protected override bool Filter(CharacterEntity entity)
			=> entity.HasHealth && entity.HealthModifier.Values.Count > 0;

		protected override void Execute(List<CharacterEntity> entities) {
			foreach (var entity in entities) {
				var health = entity.Health.Value;
				var modifiers = entity.HealthModifier.Values;
				var sum = 0f;
				foreach (var modifier in modifiers)
					sum += modifier.Value;
				health += sum;
				health = Mathf.Clamp(health, 0, entity.MaxHealth.Value);
				modifiers.Clear();
				entity.ReplaceHealthModifier(modifiers);
				entity.ReplaceHealth(health);
			}
		}
	}
}