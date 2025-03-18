using System;

namespace Ecs.Item {
	[Serializable]
	public struct ItemData {
		public ItemId Id;
		public EItemType Type;
		public int StackSize;
		public bool IsStackable => StackSize > 1;
	}
}