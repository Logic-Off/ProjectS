using System.Collections.Generic;
using UnityEngine;

namespace Ecs.Item {
	[CreateAssetMenu(menuName = "Databases/ItemsDatabase", fileName = "ItemsDatabase")]
	public sealed class ItemsDatabaseAsset : ScriptableObject {
		public List<ItemData> All;
		public List<WeaponData> Weapons;
		public List<ClothData> Clothes;
		public List<ContainerSettingsData> ContainerSettings;
		public List<ItemFilterData> ItemFilters;
		public List<ContainerFilterData> ContainerItemFilters;
	}
}