using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.Ui {
	[InstallerGenerator(InstallerId.Ui, 100500)]
	public class OnTouchSubscribersUiSystem : ReactiveSystem<UiEntity> {
		public OnTouchSubscribersUiSystem(UiContext ui) : base(ui) { }

		protected override ICollector<UiEntity> GetTrigger(IContext<UiEntity> context)
			=> context.CreateCollector(UiMatcher.TouchEvents.Added());

		protected override bool Filter(UiEntity entity)
			=> entity.HasOnTouchSubscribers;

		protected override void Execute(List<UiEntity> entities) {
			foreach (var ui in entities) {
				foreach (var listener in ui.OnTouchSubscribers.List)
				foreach (var touchEvent in ui.TouchEvents.List)
					listener(ui, touchEvent);
				ui.TouchEvents.List.Clear();
			}
		}
	}
}