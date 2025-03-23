using System.Collections.Generic;
using UnityEngine;

namespace Ecs.Common {
	
	[CreateAssetMenu(menuName = "Databases/PrefabsDatabase", fileName = "PrefabsDatabase")]
	public sealed class PrefabsDatabaseAsset : ScriptableObject {
		public List<PrefabData> All;
		public List<PrefabData> Weapons;
		public List<PrefabData> Cheats;
	}
}