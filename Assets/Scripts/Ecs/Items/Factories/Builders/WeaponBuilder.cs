
using Ecs.Ability;
using Utopia;

namespace Ecs.Item.Builders {
	[InstallerGenerator(InstallerId.Game,  50_000)]
	public class WeaponBuilder : IItemBuilder {
		private readonly IWeaponsDatabase _weapons;
		private readonly IAbilityFactory _abilityFactory;

		public WeaponBuilder(IWeaponsDatabase weapons, IAbilityFactory abilityFactory) {
			_weapons = weapons;
			_abilityFactory = abilityFactory;
		}

		public void Build(ItemEntity item) {
			if (item.ItemType.Value != EItemType.Weapon)
				return;

			var entry = _weapons.Get(item.ItemId.Value);
			item.AddBaseAbility(_abilityFactory.Create(entry.BaseAbility, item.Id.Value).Id.Value);
			item.AddWeaponType(entry.Type);
			item.AddDamageType(entry.DamageType);
			item.AddAnimationState(entry.AnimationState);
		}
	}
}