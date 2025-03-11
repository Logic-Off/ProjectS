using Common.Save;

namespace Ecs.Common {
	public interface ILocationLoadProcessor {
		void Load(LocationSave data);
	}
}