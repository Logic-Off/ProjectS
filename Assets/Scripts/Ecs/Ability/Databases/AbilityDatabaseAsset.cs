using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ecs.Ability {
	[CreateAssetMenu(menuName = "Databases/AbilityDatabase", fileName = "AbilityDatabase")]
	public sealed class AbilitiesDatabaseAsset : ScriptableObject {
		[TableList(AlwaysExpanded = true)]
		public List<AbilityData> Abilities;
	}
}