using Common;
using Utopia;
using Zentitas;

namespace Ecs.Ability {
	[InstallerGenerator(InstallerId.Game)]
	public class AbilityCooldownTickSystem : IUpdateSystem {
		private readonly IClock _clock;
		private readonly IGroup<AbilityEntity> _group;

		public AbilityCooldownTickSystem(AbilityContext ability, IClock clock) {
			_clock = clock;
			_group = ability.GetGroup(AbilityMatcher.EndCooldownTime);
		}

		public void Update() {
			var abilities = ListPool<AbilityEntity>.Get();
			var removed = ListPool<AbilityEntity>.Get();
			var time = _clock.Time;

			_group.GetEntities(abilities);
			foreach (var ability in abilities)
				if (time >= ability.EndCooldownTime.Value)
					removed.Add(ability);

			foreach (var ability in removed) {
				ability.RemoveEndCooldownTime();
				ability.IsCooldown = false;
			}

			removed.ReturnToPool();
			abilities.ReturnToPool();
		}
	}
}