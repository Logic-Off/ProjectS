using System.Collections.Generic;
using Common;
using Utopia;
using Zentitas;

namespace Ecs.Ui {
	[InstallerGenerator(InstallerId.Ui)]
	public sealed class IconChangeSystem : ReactiveSystem<UiEntity> {
		private readonly IIconsDatabase _iconsDatabase;

		public IconChangeSystem(UiContext ui, IIconsDatabase iconsDatabase) : base(ui) => _iconsDatabase = iconsDatabase;

		protected override ICollector<UiEntity> GetTrigger(IContext<UiEntity> context)
			=> context.CreateCollector(UiMatcher.IconId.Added());

		protected override bool Filter(UiEntity entity)
			=> entity.HasIconId;

		protected override void Execute(List<UiEntity> entities) {
			foreach (var entity in entities) {
				var iconId = entity.IconId.Value;
				entity.ReplaceSprite(_iconsDatabase.Get(iconId).Sprite);
			}
		}
	}
}