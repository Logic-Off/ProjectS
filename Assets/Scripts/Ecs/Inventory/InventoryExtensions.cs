using Ecs.Common;
using JetBrains.Annotations;

namespace Ecs.Inventory {
	public static class InventoryExtensions {
		[CanBeNull]
		public static InventoryEntity GetContainerByType(this InventoryContext context, Id owner, EContainerType containerType) {
			var containers = context.GetEntitiesWithOwner(owner);
			foreach (var container in containers)
				if (container.ContainerType.Value == containerType)
					return container;

			return null;
		}
	}
}