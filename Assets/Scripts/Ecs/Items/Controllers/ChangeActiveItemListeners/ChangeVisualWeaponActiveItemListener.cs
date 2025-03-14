using Ecs.Game;
using Ecs.Item;
using Utopia;

namespace Ecs.Items {
	[InstallerGenerator(InstallerId.Game)]
	public sealed class ChangeVisualWeaponActiveItemListener : IChangeActiveItemListener {
		private readonly GameContext _game;

		public ChangeVisualWeaponActiveItemListener(GameContext game) => _game = game;

		public bool CanActivate(ItemEntity item) => item.ItemType.Value is EItemType.Weapon && item.HasOwner;

		public void Activate(ItemEntity item) {
			var owner = _game.GetEntityWithId(item.Owner.Value);
			var changeItems = owner.ChangeItems.Values;
			changeItems[EItemPosition.RightHand] = item.ItemId.Value;
			owner.ReplaceChangeItems(changeItems);
		}

		public void Deactivate(ItemEntity item) {
			var owner = _game.GetEntityWithId(item.Owner.Value);
			var changeItems = owner.ChangeItems.Values;
			changeItems[EItemPosition.RightHand] = ItemId.None;
			owner.ReplaceChangeItems(changeItems);
		}
	}
}