using Ecs.Game;
using Ecs.Item;
using Utopia;

namespace Ecs.Items {
	[InstallerGenerator(InstallerId.Game)]
	public sealed class ChangeStateWeaponActiveItemListener : IChangeActiveItemListener {
		private readonly GameContext _game;

		public ChangeStateWeaponActiveItemListener(GameContext game) => _game = game;

		public bool CanActivate(ItemEntity item) => item.ItemType.Value is EItemType.Weapon && item.HasOwner;

		public void Activate(ItemEntity item) {
			var owner = _game.GetEntityWithId(item.Owner.Value);
			owner?.Animator.Value.SetBool(item.AnimationState.Value, true);
		}

		public void Deactivate(ItemEntity item) {
			var owner = _game.GetEntityWithId(item.Owner.Value);
			owner?.Animator.Value.SetBool(item.AnimationState.Value, false);
		}
	}
}