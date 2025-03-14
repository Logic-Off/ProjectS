using Ecs.Character;
using Ecs.Inventory;
using Utopia;

namespace Ecs.Items {
	[InstallerGenerator(InstallerId.Game)]
	public sealed class ChangeActiveBuffItemListener : IChangeActiveItemListener {
		private readonly CharacterContext _character;
		private readonly InventoryContext _inventory;
		private readonly ItemContext _items;

		public ChangeActiveBuffItemListener(CharacterContext character, InventoryContext inventory, ItemContext items) {
			_character = character;
			_inventory = inventory;
			_items = items;
		}

		public bool CanActivate(ItemEntity item) => item.HasBuffs;

		public void Activate(ItemEntity item) {
			var owner = _character.GetEntityWithId(item.Owner.Value);
			if (owner == null)
				return;

			var buffs = owner.BuffModifier.Values;
			foreach (var buff in item.Buffs.Values)
				buffs.Add(new BuffModifier(buff, EBuffModifier.Added));

			owner.ReplaceBuffModifier(buffs);
		}

		public void Deactivate(ItemEntity item) {
			var owner = _character.GetEntityWithId(item.Owner.Value);
			if (owner == null)
				return;
			var buffs = owner.BuffModifier.Values;
			foreach (var buff in item.Buffs.Values)
				buffs.Add(new BuffModifier(buff, EBuffModifier.Removed));

			owner.ReplaceBuffModifier(buffs);
		}
	}
}