using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.Ui {
	[InstallerGenerator(InstallerId.Ui, 5_000_000)]
	public sealed class ButtonClickedDisableSystem : ReactiveSystem<UiEntity> {
		public ButtonClickedDisableSystem(UiContext ui) : base(ui) { }

		protected override ICollector<UiEntity> GetTrigger(IContext<UiEntity> context)
			=> context.CreateCollector(UiMatcher.Clicked.Added());

		protected override bool Filter(UiEntity entity)
			=> entity.HasClicked;

		protected override void Execute(List<UiEntity> entities) {
			foreach (var entity in entities) {
				entity.IsClicked = false;
			}
		}
	}
}