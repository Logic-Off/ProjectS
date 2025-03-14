using System.Collections.Generic;
using Common;
using Utopia;
using Zentitas;

namespace Ecs.Character {
	[InstallerGenerator(InstallerId.Game, 1_000_000)]
	public class CharacterStatModifierSystem : ReactiveSystem<CharacterEntity> {
		public CharacterStatModifierSystem(CharacterContext character) : base(character) { }

		protected override ICollector<CharacterEntity> GetTrigger(IContext<CharacterEntity> context)
			=> context.CreateCollector(CharacterMatcher.StatModifier.Added());

		protected override bool Filter(CharacterEntity entity)
			=> entity.HasStatModifier && entity.StatModifier.Values.Count > 0;

		protected override void Execute(List<CharacterEntity> entities) {
			foreach (var entity in entities) {
				var modifiers = entity.StatModifier.Values;
				foreach (var modifier in modifiers)
					OnChangeStat(entity, modifier);
				modifiers.Clear();
				entity.ReplaceStatModifier(modifiers);
			}
		}

		private void OnChangeStat(CharacterEntity entity, StatModifier modifier) {
			switch (modifier.Stat) {
				case ECharacterStat.Health:
					var health = (entity.Health.Value + modifier.Value).Clamp(0, entity.MaxHealth.Value);
					entity.ReplaceHealth(health);
					break;
				case ECharacterStat.Hunger when entity.HasHunger:
					var hunger = (entity.Hunger.Value + modifier.Value).Clamp(0, entity.MaxHunger.Value);
					entity.ReplaceHunger(hunger);
					break;
				case ECharacterStat.Thirst when entity.HasThirst:
					var thirst = (entity.Thirst.Value + modifier.Value).Clamp(0, entity.MaxThirst.Value);
					entity.ReplaceThirst(thirst);
					break;
				case ECharacterStat.Psyche when entity.HasPsyche:
					var psyche = (entity.Psyche.Value + modifier.Value).Clamp(0, entity.MaxPsyche.Value);
					entity.ReplacePsyche(psyche);
					break;
				case ECharacterStat.Cold when entity.HasCold:
					var cold = (entity.Cold.Value + modifier.Value).Clamp(0, entity.MaxCold.Value);
					entity.ReplaceCold(cold);
					break;
				case ECharacterStat.Radiation when entity.HasRadiation:
					var radiation = (entity.Radiation.Value + modifier.Value).Clamp(0, entity.MaxRadiation.Value);
					entity.ReplaceRadiation(radiation);
					break;
			}
		}
	}
}