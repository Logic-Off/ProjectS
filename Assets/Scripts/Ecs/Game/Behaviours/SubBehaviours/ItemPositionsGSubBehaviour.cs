using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ecs.Game {
	public sealed class ItemPositionsGSubBehaviour : AGameSubBehaviour {
		[SerializeField] private List<Position> _positions;

		public override void Link(GameEntity entity) {
			var positions = new Dictionary<EItemPosition, Transform>();
			foreach (var position in _positions)
				positions.Add(position.Type, position.Value);
			entity.AddItemTransformPositions(positions);
		}

		[Serializable]
		private struct Position {
			public EItemPosition Type;
			public Transform Value;
		}
	}
}