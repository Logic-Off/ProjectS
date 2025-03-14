using System;
using UnityEngine;
using Utopia;
using Zentitas;

namespace Ecs.Character {
	[InstallerGenerator(InstallerId.Game)]
	public class BuffTickSystem : IUpdateSystem {
		private float _pause;
		private readonly IGroup<CharacterEntity> _group;

		public BuffTickSystem(CharacterContext character)
			=> _group = character.GetGroup(CharacterMatcher.AllOf(CharacterMatcher.Parameters, CharacterMatcher.Buffs).NoneOf(CharacterMatcher.Dead));

		public void Update() {
			var time = Time.realtimeSinceStartup;
			if (_pause > time)
				return;
			_pause = time + 1;

			var buffer = ListPool<CharacterEntity>.Get();
			_group.GetEntities(buffer);

			foreach (var entity in buffer) {
				foreach (var buff in entity.Buffs.Values) {
					if (buff.IsDisable || buff.Type is not EBuffType.Ticked)
						continue;

					OnTick(entity, buff);
				}
			}

			ListPool<CharacterEntity>.Release(buffer);
		}

		private void OnTick(CharacterEntity entity, ABuff buff) {
			foreach (var effect in buff.Effects) {
				switch (effect.Type) {
					case ECharacterStat.Health:
						entity.AddModifier(
							ECharacterStat.Health,
							new StatModifier() {
								Type = effect.Parameter.Value > 0 ? EStatModifierType.Heal : EStatModifierType.Damage,
								Value = effect.Parameter.Value
							}
						);
						break;
					case ECharacterStat.Stealth:
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
	}
}