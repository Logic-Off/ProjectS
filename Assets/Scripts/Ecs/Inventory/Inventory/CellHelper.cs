using Ecs.Item;
using Utopia;

namespace Ecs.Inventory {
	[InstallerGenerator(InstallerId.Game)]
	public sealed class CellHelper : ICellHelper {
		private readonly ItemContext _item;

		public CellHelper(ItemContext item) => _item = item;

		public void SetItem(InventoryEntity cell, ItemEntity target, int quantity) {
			target.ReplaceCellId(cell.CellId.Value);
			target.ReplaceQuantity(quantity);
			cell.ReplaceCellTarget(target.ItemInstanceId.Value);
			cell.IsEmpty = quantity == 0;
			cell.IsChanged = true;
		}

		public void Add(InventoryEntity cell, int count = 1) {
			var target = _item.GetEntityWithItemInstanceId(cell.CellTarget.Value);
			target.ReplaceQuantity(target.Quantity.Value + count);
			cell.IsChanged = true;
			cell.IsEmpty = target.Quantity.Value == 0;
		}

		public void Remove(InventoryEntity cell, int count = 1) {
			var target = _item.GetEntityWithItemInstanceId(cell.CellTarget.Value);
			target.ReplaceQuantity(target.Quantity.Value - count);
			cell.IsChanged = true;
			if (target.Quantity.Value > 0)
				return;
			// Необходимо в этот же кадр удалить ячейку, потому что ячейка может перезаписаться и присвоиться другому итему
			target.RemoveCellId();
			Reset(cell);
		}

		public bool Contains(InventoryEntity cell, ItemId itemId) {
			var target = _item.GetEntityWithItemInstanceId(cell.CellTarget.Value);
			if (target == null)
				return false;
			return target.ItemId.Value == itemId;
		}

		public void Destroy(InventoryEntity cell) {
			var target = _item.GetEntityWithItemInstanceId(cell.CellTarget.Value);
			target.ReplaceQuantity(target.Quantity.Value);
			target.RemoveCellId();
			Reset(cell);
		}

		public int Quantity(InventoryEntity cell) {
			var target = _item.GetEntityWithItemInstanceId(cell.CellTarget.Value);
			return target == null ? 0 : target.Quantity.Value;
		}

		public ItemId GetItemId(InventoryEntity cell) {
			var target = _item.GetEntityWithItemInstanceId(cell.CellTarget.Value);
			return target == null ? ItemId.None : target.ItemId.Value;
		}

		public void Reset(InventoryEntity cell) {
			cell.ReplaceCellTarget(ItemInstanceId.None);
			cell.IsEmpty = true;
			cell.IsChanged = true;
		}
	}
}