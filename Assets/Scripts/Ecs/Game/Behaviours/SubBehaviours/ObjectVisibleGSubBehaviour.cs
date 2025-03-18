using System.Collections.Generic;
using UnityEngine;

namespace Ecs.Game {
	public sealed class ObjectVisibleGSubBehaviour : AGameSubBehaviour {
		[SerializeField] private List<GameObject> _targets;
		
		public override void Link(GameEntity entity) {
			entity.SubscribeOnVisibleChange(OnChangeVisible);
		}

		private void OnChangeVisible(GameEntity entity) {
			foreach (var target in _targets)
				target.SetActive(entity.IsVisible);
		}
	}
}