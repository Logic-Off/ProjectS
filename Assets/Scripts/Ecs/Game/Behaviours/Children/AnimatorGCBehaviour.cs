using UnityEngine;

namespace Ecs.Game.Children {
	public sealed class AnimatorGCBehaviour : AGameSubBehaviour {
		[SerializeField] private Animator _animator;

		public override void Link(GameEntity entity) => entity.AddAnimator(_animator);
	}
}