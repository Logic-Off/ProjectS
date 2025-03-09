using System;
using Ecs.Character;
using UnityEngine;

namespace Ecs.Game {
	[Serializable]
	public struct TeamLayerMaskData {
		public ETeam Team;

		[Header("Выставляемый слой основному объекту")]
		public LayerMask Mask;

		public int MaskIndex;
	}
}