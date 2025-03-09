using System.Collections.Generic;
using UnityEngine;

namespace Ecs.Ability {
	[CreateAssetMenu(menuName = "Databases/AbilityDatabase", fileName = "AbilityDatabase")]
	public sealed class AbilitiesDatabaseAsset : ScriptableObject {
		public List<AbilityData> Abilities;
	}
}