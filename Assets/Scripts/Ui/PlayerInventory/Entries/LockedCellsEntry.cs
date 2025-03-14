using System.Collections.Generic;
using Ecs.Common;

namespace Ui.PlayerInventory {
	public sealed class LockedCellsEntry {
		public readonly Dictionary<EUiContainerType, List<Id>> Values = new();
		public void Add(EUiContainerType containerType, List<Id> list) => Values.Add(containerType, list);
		public List<Id> Get(EUiContainerType container) => Values[container];
	}
}