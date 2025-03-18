using System;

namespace Ecs.Item {
	[Serializable]
	public struct CellSettingsData {
		public ECellType Type;
		public int Quantity;
	}
}