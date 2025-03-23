using System.Collections.Generic;
using UnityEngine;

namespace Common {
	[CreateAssetMenu(fileName = "IconsDatabase", menuName = "Databases/IconsDatabase")]
	public sealed class IconsDatabaseAsset : ScriptableObject {
		public List<IconData> Icons;
		public List<IconData> WeaponIcons;
	}
}