using Cysharp.Threading.Tasks;
using Ecs.Common;
using UnityEngine;

namespace Ui.Cheats {
	public interface ICheatHeaderPool {
		UniTask<UiEntity> Get(Id parentId, Transform container, string name);
		void Return(UiEntity entity);
	}
}