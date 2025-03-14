using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ui {
	public abstract class APrebuildWindow : AWindow, IPrebuildWindow {
		public async UniTask Prebuild() => await BuildPanels(_ui, _entity);
	}
}