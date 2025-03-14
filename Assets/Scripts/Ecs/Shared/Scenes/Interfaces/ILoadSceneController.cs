using Cysharp.Threading.Tasks;

namespace Ecs.Shared {
	public interface ILoadSceneController {
		UniTask OnLoad(LocationId locationId);
	}
}