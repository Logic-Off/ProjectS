using System.Collections.Generic;
using Ecs.Ability;
using UnityEditor;

namespace LogicOff.IdDrawer {
	[CustomPropertyDrawer(typeof(AbilityId))]
	public class AbilityIdDrawer : AIdDrawer<AbilitiesDatabaseAsset, AbilityData> {
		protected override string Name => "Ability";
		protected override string Path => "Assets/Settings/Databases/AbilityDatabase.asset";

		protected override IEnumerable<AbilityData> GetList(AbilitiesDatabaseAsset database) {
			var list = new List<AbilityData>();
			list.AddRange(database.Abilities);
			return list;
		}

		protected override string GetNameFromItem(AbilityData value) => value.Id;
	}
}