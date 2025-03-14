
using Utopia;

namespace Ecs.Item.Builders {
	[InstallerGenerator(InstallerId.Game,  50_000)]
	public class WeaponBuilder : IItemBuilder {
		private readonly IWeaponsDatabase _weapons;

		public WeaponBuilder(IWeaponsDatabase weapons) => _weapons = weapons;

		public void Build(ItemEntity item) {
			if (item.ItemType.Value != EItemType.Weapon)
				return;

			var entry = _weapons.Get(item.ItemId.Value);
			item.AddBaseAbility(entry.BaseAbility);
		}
	}
}