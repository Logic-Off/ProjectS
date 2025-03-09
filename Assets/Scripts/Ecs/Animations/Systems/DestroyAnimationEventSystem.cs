using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.Animations {
	[InstallerGenerator(InstallerId.Game)]
	public class DestroyAnimationEventSystem : ReactiveSystem<AnimationEventEntity> {
		public DestroyAnimationEventSystem(AnimationEventContext animationEvent) : base(animationEvent) { }

		protected override ICollector<AnimationEventEntity> GetTrigger(IContext<AnimationEventEntity> context)
			=> context.CreateCollector(AnimationEventMatcher.Destroyed.Added());

		protected override bool Filter(AnimationEventEntity entity)
			=> entity.IsDestroyed;

		protected override void Execute(List<AnimationEventEntity> entities) {
			foreach (var entity in entities)
				entity.Destroy();
		}
	}
}