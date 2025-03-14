using System.Collections.Generic;
using Ecs.Common;
using Ecs.Inventory;

namespace Ui.PlayerInventory {
	public sealed class UnlockedCellsEntry {
		public readonly Dictionary<EUiContainerType, Dictionary<CellId, Id>> Values = new();

		public void Add(EUiContainerType containerType, Dictionary<CellId, Id> dictionary) => Values.Add(containerType, dictionary);

		public void Clear() {
			foreach (var pair in Values)
				pair.Value.Clear();
		}

		public Dictionary<CellId, Id> Get(EUiContainerType container) => Values[container];
	}
}