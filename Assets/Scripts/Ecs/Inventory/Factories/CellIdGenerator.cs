using Utopia;

namespace Ecs.Inventory {
	[InstallerGenerator(InstallerId.Game)]
	public class CellIdGenerator {
		private int _index = 1000;

		public CellId Next() => new(_index++);
	}
}