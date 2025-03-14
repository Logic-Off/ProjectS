using System;

namespace Ecs.Item {
	[Serializable]
	public struct CellSettingsEntry {
		public ECellType Type;
		public int Quantity;
	}
}