using System.Collections.Generic;
using UnityEngine;

namespace Ecs.Item {
	[CreateAssetMenu(menuName = "Databases/ItemsDatabase", fileName = "ItemsDatabase")]
	public sealed class ItemsDatabaseAsset : ScriptableObject {
		public List<ItemEntry> All;
		public List<WeaponEntry> Weapons;
		public List<ClothEntry> Clothes;
		public List<ContainerSettingsEntry> ContainerSettings;
		public List<ItemFilterEntry> ItemFilters;
		public List<ContainerFilterEntry> ContainerItemFilters;
	}
}