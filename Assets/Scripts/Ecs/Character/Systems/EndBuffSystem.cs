using System.Collections.Generic;
using UnityEngine;
using Utopia;
using Zentitas;

namespace Ecs.Character {
	[InstallerGenerator(InstallerId.Game)]
	public class EndBuffSystem : ICleanupSystem {
		private readonly IGroup<CharacterEntity> _group;

		public EndBuffSystem(CharacterContext arbiter) => _group = arbiter.GetGroup(CharacterMatcher.Buffs);

		public void Cleanup() {
			var buffer = ListPool<CharacterEntity>.Get();
			var remove = ListPool<ABuff>.Get();
			_group.GetEntities(buffer);

			var time = Time.realtimeSinceStartup;

			foreach (var entity in buffer) {
				CollectBuffs(entity, time, remove);
				RemoveBuffs(entity, remove);
				remove.Clear();
			}

			remove.ReturnToPool();
			buffer.ReturnToPool();
		}

		private void CollectBuffs(CharacterEntity entity, float time, List<ABuff> remove) {
			foreach (var buff in entity.Buffs.Values)
				if (buff.IsDisable || buff.Type is EBuffType.Temporary or EBuffType.Ticked && buff.EndTime < time)
					remove.Add(buff);
		}

		private void RemoveBuffs(CharacterEntity entity, List<ABuff> remove) {
			if (remove.Count == 0)
				return;
			var buffModifiers = entity.BuffModifier.Values;
			foreach (var buff in remove)
				buffModifiers.Add(new BuffModifier(buff, EBuffModifier.Removed));

			entity.ReplaceBuffModifier(buffModifiers);
		}
	}
}