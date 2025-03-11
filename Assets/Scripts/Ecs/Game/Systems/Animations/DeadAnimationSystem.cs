using System.Collections.Generic;
using UnityEngine;
using Utopia;
using Zentitas;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public sealed class DeadAnimationSystem : ReactiveSystem<GameEntity> {
		private readonly int Dead = Animator.StringToHash("Dead");
		
		public DeadAnimationSystem(GameContext game) : base(game) { }

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
			=> context.CreateCollector(GameMatcher.Dead.AddedOrRemoved(), GameMatcher.Animator.Added());

		protected override bool Filter(GameEntity entity)
			=> entity.HasDead && entity.HasAnimator;

		protected override void Execute(List<GameEntity> entities) {
			foreach (var entity in entities)
				entity.Animator.Value.SetBool(Dead, entity.IsDead);
		}
	}
}