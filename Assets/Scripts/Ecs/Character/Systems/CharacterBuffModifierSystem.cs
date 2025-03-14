using System;
using System.Collections.Generic;
using Common;
using Utopia;
using Zentitas;

namespace Ecs.Character {
	[InstallerGenerator(InstallerId.Game)]
	public class CharacterBuffModifierSystem : ReactiveSystem<CharacterEntity> {
		private readonly GameContext _game;

		public CharacterBuffModifierSystem(CharacterContext character, GameContext game) : base(character) => _game = game;

		protected override ICollector<CharacterEntity> GetTrigger(IContext<CharacterEntity> context)
			=> context.CreateCollector(CharacterMatcher.BuffModifier.Added());

		protected override bool Filter(CharacterEntity entity)
			=> entity.HasBuffModifier;

		protected override void Execute(List<CharacterEntity> entities) {
			foreach (var entity in entities) {
				OnBuffsChange(entity);

				entity.BuffModifier.Values.Clear();
			}
		}

		private void OnBuffsChange(CharacterEntity entity) {
			foreach (var modifier in entity.BuffModifier.Values) {
				if (modifier.Buff.Type is EBuffType.OneShot) {
					if (modifier.Modifier is not EBuffModifier.Added)
						continue;
					ActivateEffects(entity, modifier.Buff.Effects);
					ChangeTriggers(entity, modifier.Buff.Triggers, true);
				} else {
					if (modifier.Modifier == EBuffModifier.Added)
						ActivateBuff(entity, modifier.Buff);
					else
						DeactivateBuff(entity, modifier.Buff);
				}
			}

			UpdateTriggers(entity);
		}

		private void ActivateBuff(CharacterEntity entity, ABuff buff) {
			ActivateEffects(entity, buff.Effects);

			var buffs = entity.Buffs.Values;
			buffs.Add(buff);
			entity.ReplaceBuffs(buffs);
		}

		private void ActivateEffects(CharacterEntity entity, List<Effect> effects) {
			var character = entity.Parameters.Value;
			foreach (var effect in effects) {
				switch (effect.Type) {
					case ECharacterStat.MaxHealth:
						entity.ReplaceMaxHealth(entity.MaxHealth.Value + effect.Parameter);
						entity.ReplaceHealth(character.Health.Value.Min(entity.MaxHealth.Value));
						break;
					default:
						throw new ArgumentOutOfRangeException($"{effect.Type}");
				}
			}

			entity.ReplaceParameters(character);
		}

		private void ChangeTriggers(CharacterEntity entity, List<TriggerEffect> disableTriggers, bool isActivate) {
			var agent = _game.GetEntityWithId(entity.Id.Value);
			foreach (var trigger in disableTriggers) {
				switch (trigger.Type) {
					case ETriggerEffect.Stun:
						agent.IsStun = trigger.Value == isActivate;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		// Апдейт необходим для того, чтоб вернуть отключенные триггеры
		private void UpdateTriggers(CharacterEntity entity) {
			var agent = _game.GetEntityWithId(entity.Id.Value);
			foreach (var buff in entity.Buffs.Values) {
				foreach (var trigger in buff.Triggers) {
					switch (trigger.Type) {
						case ETriggerEffect.Stun:
							agent.IsStun = trigger.Value;
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}
			}
		}

		private void DeactivateBuff(CharacterEntity entity, ABuff buff) {
			var parameters = entity.Parameters.Value;
			foreach (var effect in buff.Effects) {
				switch (effect.Type) {
					case ECharacterStat.MaxHealth:
						entity.ReplaceMaxHealth(entity.MaxHealth.Value - effect.Parameter);
						entity.ReplaceHealth(entity.Health.Value.Min(entity.MaxHealth.Value));
						break;
					default:
						throw new ArgumentOutOfRangeException($"{effect.Type}");
				}
			}

			entity.ReplaceParameters(parameters);
			ChangeTriggers(entity, buff.Triggers, false);
			var buffs = entity.Buffs.Values;
			buffs.Remove(buff);
			entity.ReplaceBuffs(buffs);
		}
	}
}