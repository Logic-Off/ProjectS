using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Ecs.Common;
using Ecs.Ui;
using UnityEngine;
using Utopia;

namespace Ui.Cheats {
	[InstallerGenerator(InstallerId.Cheats)]
	public class CheatButtonPool : AUiPool, ICheatButtonPool {
		private int _index = 0;
		protected override string PrefabName => "CheatButton";
		public CheatButtonPool(IPrefabsDatabase database, ICanvasParent parent, UiContext ui) : base(database, parent, ui) { }

		public async UniTask<UiEntity> Get(Id parentId, Transform container, string name) {
			var entity = await base.Get(parentId, container, name);
			return entity;
		}

		public override void Return(UiEntity entity) {
			entity.OnClickedChangeEventActionListeners.Values.Clear();
			base.Return(entity);
		}
	}
}