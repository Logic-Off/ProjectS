using Cysharp.Threading.Tasks;
using Ecs.Common;
using Ecs.Ui;
using UnityEngine;
using Utopia;

namespace Ui.Cheats {
	[InstallerGenerator(InstallerId.Cheats)]
	public class CheatHeaderPool : AUiPool, ICheatHeaderPool {
		private int _index = 0;
		protected override string PrefabName => "CheatHeader";
		public CheatHeaderPool(IPrefabsDatabase database, ICanvasParent parent, UiContext ui) : base(database, parent, ui) { }

		public async UniTask<UiEntity> Get(Id parentId, Transform container, string name) {
			var entity = await base.Get(parentId, container, name);
			var rect = entity.Rect.Value;
			rect.sizeDelta = new Vector2(rect.parent.GetComponent<RectTransform>().rect.width, rect.sizeDelta.y);
			return entity;
		}
	}
}