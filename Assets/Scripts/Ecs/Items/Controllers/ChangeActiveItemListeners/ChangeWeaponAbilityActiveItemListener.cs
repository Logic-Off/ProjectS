using Ecs.Item;
using Utopia;

namespace Ecs.Items {
	[InstallerGenerator(InstallerId.Game)]
	public sealed class ChangeWeaponAbilityActiveItemListener : IChangeActiveItemListener {
		private readonly GameContext _game;

		public ChangeWeaponAbilityActiveItemListener(GameContext game)
			=> _game = game;

		public bool CanActivate(ItemEntity item) => item.ItemType.Value is EItemType.Weapon;

		public void Activate(ItemEntity item) {
			var owner = _game.GetEntityWithId(item.Owner.Value);
			if (owner == null)
				return;
			owner.ReplaceBaseAbility(item.BaseAbility.Value);
			var abilities = owner.Abilities.Values;
			abilities.Add(item.BaseAbility.Value);
			owner.ReplaceAbilities(abilities);
			owner.ReplaceCurrentWeapon(item.ItemInstanceId.Value);
		}

		public void Deactivate(ItemEntity item) {
			var owner = _game.GetEntityWithId(item.Owner.Value);
			if (owner == null)
				return;
			if (owner.HasDefaultAbility)
				owner.ReplaceBaseAbility(owner.DefaultAbility.Value);
			else
				owner.RemoveBaseAbility();
			
			var abilities = owner.Abilities.Values;
			abilities.Remove(item.BaseAbility.Value);
			owner.ReplaceAbilities(abilities);
			owner.RemoveCurrentWeapon();
		}
	}
}