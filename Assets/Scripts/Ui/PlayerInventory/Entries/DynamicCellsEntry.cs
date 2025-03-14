
using Ecs.Inventory;

namespace Ui.PlayerInventory {
	public sealed class DynamicCellsEntry {
		public readonly EContainerType ContainerType;
		/// <summary>
		/// Контейнер куда будет помещена ячейка
		/// </summary>
		public readonly EUiContainerType RectContainerType;
		/// <summary>
		/// Тип контейнера
		/// </summary>
		public readonly EUiContainerType UiItemContainer;

		public DynamicCellsEntry(EContainerType containerType, EUiContainerType rectContainerType, EUiContainerType uiItemContainer) {
			ContainerType = containerType;
			RectContainerType = rectContainerType;
			UiItemContainer = uiItemContainer;
		}
	}
}