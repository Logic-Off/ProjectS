using Cysharp.Threading.Tasks;
using Ecs.Common;
using UnityEngine;

namespace Ui.Cheats {
	public interface ISmallCheatButtonPool {
		UniTask<UiEntity> Get(Id parentId, Transform container, ICheat cheat);
		void Return(UiEntity entity);
	}
}