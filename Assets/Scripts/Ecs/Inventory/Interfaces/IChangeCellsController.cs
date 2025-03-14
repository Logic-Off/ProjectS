using Ecs.Common;

namespace Ecs.Inventory {
	public interface IChangeCellsController {
		void OnChangeCellSettings(Id owner, EContainerType containerType);
		void OnChangeCellSettings(InventoryEntity container);
	}
}