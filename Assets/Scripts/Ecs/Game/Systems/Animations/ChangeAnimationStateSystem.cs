using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public sealed class ChangeAnimationStateSystem : ReactiveSystem<GameEntity> {
		public ChangeAnimationStateSystem(GameContext game) : base(game) { }

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
			=> context.CreateCollector(GameMatcher.CurrentAnimationState.AddedOrRemoved(), GameMatcher.Animator.Added());

		protected override bool Filter(GameEntity entity)
			=> entity.HasAnimator && entity.HasCurrentAnimationState;

		protected override void Execute(List<GameEntity> entities) {
			foreach (var entity in entities) {
				var animator = entity.Animator.Value;
				var newState = entity.CurrentAnimationState.Value;

				animator.SetBool(entity.PreviousAnimationState.Value, false);
				animator.SetBool(newState, true);

				entity.ReplacePreviousAnimationState(newState);
			}
		}
	}
}