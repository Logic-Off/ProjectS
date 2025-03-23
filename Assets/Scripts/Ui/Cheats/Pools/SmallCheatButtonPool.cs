using Cysharp.Threading.Tasks;
using Ecs.Common;
using Ecs.Ui;
using UnityEngine;
using Utopia;

namespace Ui.Cheats {
	[InstallerGenerator(InstallerId.Cheats)]
	public class SmallCheatButtonPool : AUiPool, ISmallCheatButtonPool {
		protected override string PrefabName => "SmallCheatButton";
		public SmallCheatButtonPool(IPrefabsDatabase database, ICanvasParent parent, UiContext ui) : base(database, parent, ui) { }

		public async UniTask<UiEntity> Get(Id parentId, Transform container, ICheat cheat) {
			var entity = await base.Get(parentId, container, cheat.Name);
			entity.AddString(cheat.Name);
			return entity;
		}

		public override void Return(UiEntity entity) {
			entity.OnClickedChangeEventActionListeners.Values.Clear();
			base.Return(entity);
		}
	}
}