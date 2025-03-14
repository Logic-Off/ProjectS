using Ecs.Common;
using Utopia;

namespace Ecs.Items {
	[InstallerGenerator(InstallerId.Game)]
	public class ActivateItemController {
		private readonly IChangeActiveItemController _changeActiveItemController;

		public ActivateItemController(IChangeActiveItemController changeActiveItemController) => _changeActiveItemController = changeActiveItemController;

		public void ActivateItem(ItemEntity item, Id owner) {
			if (item.HasOwner)
				return;

			item.AddOwner(owner);
			_changeActiveItemController.Activate(item);
		}

		public void DeactivateItem(ItemEntity item) {
			if (!item.HasOwner)
				return;

			_changeActiveItemController.Deactivate(item);
			item.RemoveOwner();
		}
	}
}