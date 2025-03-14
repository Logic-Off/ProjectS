using Ecs.Inventory;
using Utopia;

namespace Ecs.Item {
	[InstallerGenerator(InstallerId.Game)]
	public class ItemInstanceIdGenerator {
		private int _index = 1000;

		public ItemInstanceId Next() => new(_index++);
	}
}