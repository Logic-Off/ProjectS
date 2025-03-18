using System.Collections.Generic;
using UnityEngine;
using Utopia;
using Zentitas;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public class SetRotationSystem : ReactiveSystem<GameEntity> {
		public SetRotationSystem(GameContext game) : base(game) { }

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
			=> context.CreateCollector(GameMatcher.NewRotation);

		protected override bool Filter(GameEntity entity)
			=> entity.HasNewRotation && entity.HasTransform;

		protected override void Execute(List<GameEntity> entities) {
			foreach (var entity in entities) {
				var rotation = new Vector3(0, entity.NewRotation.Value.eulerAngles.y, 0);
				entity.Transform.Value.rotation = Quaternion.Euler(rotation);
			}
		}
	}
}