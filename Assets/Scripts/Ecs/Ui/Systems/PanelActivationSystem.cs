using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.Ui {
	[InstallerGenerator(InstallerId.Ui, 100_100)]
	public class PanelActivationSystem : ReactiveSystem<UiEntity> {
		private readonly UiContext _ui;
		public PanelActivationSystem(UiContext ui) : base(ui) => _ui = ui;

		protected override ICollector<UiEntity> GetTrigger(IContext<UiEntity> context)
			=> context.CreateCollector(UiMatcher.Active.AddedOrRemoved());

		protected override bool Filter(UiEntity entity)
			=> entity.HasUiType && entity.UiType.Value is EUiType.Panel;

		protected override void Execute(List<UiEntity> entities) {
			foreach (var entity in entities)
				ActivateChildren(entity);
		}

		private void ActivateChildren(UiEntity entity) {
			var children = _ui.GetEntitiesWithParent(entity.Id.Value);
			foreach (var child in children) {
				child.IsActive = entity.IsActive;
				ActivateChildren(child);
			}
		}
	}
}