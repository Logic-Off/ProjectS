using System;
using System.Collections.Generic;

namespace Ecs.Item {
	[Serializable]
	public struct ContainerSettingsData {
		public CellSettingsId Id;
		public List<CellSettingsData> Cells;
	}
}