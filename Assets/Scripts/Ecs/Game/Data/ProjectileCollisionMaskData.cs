using System;
using Ecs.Character;
using UnityEngine;

namespace Ecs.Game {
	[Serializable]
	public struct ProjectileCollisionMaskData {
		public ETeam Team; // Кто запустил

		[Header("С каким слоем он точно должен столкнуться, независимо от настроек проекта")]
		public LayerMask Include;

		[Header("Какой слой исключается, независимо от настроек проекта")]
		public LayerMask Exclude;
	}
}