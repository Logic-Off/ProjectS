using System.Threading.Tasks;

namespace Ecs.Shared {
	public interface ILoadSceneController {
		Task OnLoad(LocationId locationId);
	}
}