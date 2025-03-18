using UnityEngine;

namespace Ecs.Game {
	public sealed class AnimatorGSubBehaviour : AGameSubBehaviour {
		[SerializeField] private Animator _animator;

		public override void Link(GameEntity entity) => entity.AddAnimator(_animator);
	}
}