using Cysharp.Threading.Tasks;
using Ecs.Common;
using Ecs.Ui;
using UnityEngine;
using Utopia;

namespace Ui.Cheats {
	[InstallerGenerator(InstallerId.Cheats)]
	public class CheatTextPool : AUiPool, ICheatTextPool {
		private int _index = 0;
		protected override string PrefabName => "CheatText";
		public CheatTextPool(IPrefabsDatabase database, ICanvasParent parent, UiContext ui) : base(database, parent, ui) { }

		public async UniTask<UiEntity> Get(Id parentId, Transform container, string name) {
			var entity = await base.Get(parentId, container, name);
			var rect = entity.Rect.Value;
			rect.sizeDelta = new Vector2(rect.parent.GetComponent<RectTransform>().rect.width, rect.sizeDelta.y);
			return entity;
		}
	}
}