using System;
using System.Collections.Generic;

namespace Ecs.Item {
	[Serializable]
	public struct ItemFilterEntry {
		public EItemType ContainerType;
		public List<EItemType> ItemTypes;
	}
}