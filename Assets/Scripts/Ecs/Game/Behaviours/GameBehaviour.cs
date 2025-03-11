using Ecs.Common;

namespace Ecs.Game {
	public sealed class GameBehaviour : AEcsBehaviour<GameEntity, AGameSubBehaviour> {
		public override void Link(GameEntity entity) {
			entity.AddTransform(transform);
			base.Link(entity);
		}
	}
}