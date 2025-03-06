using Ecs.Common;

namespace Ecs.Game {
	public sealed class GameBehaviour : AEcsBehaviour<GameEntity, AGameChildBehaviour> {
		public override void Link(GameEntity entity) {
			entity.AddTransform(transform);
			base.Link(entity);
		}
	}
}