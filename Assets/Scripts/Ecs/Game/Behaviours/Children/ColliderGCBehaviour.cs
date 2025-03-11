using UnityEngine;

namespace Ecs.Game.Children {
	public sealed class ColliderGCBehaviour : AGameSubBehaviour {
		[SerializeField] private Collider _collider;

		public override void Link(GameEntity entity) => entity.AddCollider(_collider);
	}
}