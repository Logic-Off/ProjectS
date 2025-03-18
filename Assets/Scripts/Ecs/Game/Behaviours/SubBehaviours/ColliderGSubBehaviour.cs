using UnityEngine;

namespace Ecs.Game {
	public sealed class ColliderGSubBehaviour : AGameSubBehaviour {
		[SerializeField] private Collider _collider;

		public override void Link(GameEntity entity) => entity.AddCollider(_collider);
	}
}