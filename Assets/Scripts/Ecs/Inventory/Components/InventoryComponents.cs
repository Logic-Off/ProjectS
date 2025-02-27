using Zentitas;

namespace Ecs.Inventory {
	[Inventory]
	public sealed class ContainerType : IComponent {
		public EContainerType Value;
	}
}