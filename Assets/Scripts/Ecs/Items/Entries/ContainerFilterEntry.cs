using System;
using System.Collections.Generic;
using Ecs.Inventory;

namespace Ecs.Item {
	[Serializable]
	public struct ContainerFilterEntry {
		public EContainerType ContainerType;
		public List<EItemType> ItemTypes;
	}
}