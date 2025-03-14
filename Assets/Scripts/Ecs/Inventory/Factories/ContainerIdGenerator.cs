using Utopia;

namespace Ecs.Inventory {
	[InstallerGenerator(InstallerId.Game)]
	public class ContainerIdGenerator {
		private int _index = 1000;

		public ContainerId Next() => new(_index++);
	}
}