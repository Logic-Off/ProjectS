using System;
using System.Collections.Generic;

namespace Ecs.Item {
	[Serializable]
	public struct ContainerSettingsEntry {
		public CellSettingsId Id;
		public List<CellSettingsEntry> Cells;
	}
}