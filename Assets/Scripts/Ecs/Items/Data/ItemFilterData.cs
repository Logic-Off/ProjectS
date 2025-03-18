using System;
using System.Collections.Generic;

namespace Ecs.Item {
	[Serializable]
	public struct ItemFilterData {
		public EItemType ContainerType;
		public List<EItemType> ItemTypes;
	}
}