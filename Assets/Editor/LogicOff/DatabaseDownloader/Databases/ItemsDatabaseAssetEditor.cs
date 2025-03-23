using System.Collections.Generic;
using Ecs.Item;
using UnityEditor;

namespace LogicOff.DatabaseDownloader {
	[CustomEditor(typeof(ItemsDatabaseAsset))]
	public sealed class ItemsDatabaseAssetEditor : ADatabaseDownloader {
		private ItemsDatabaseAsset _target;

		protected override void OnEnable() {
			base.OnEnable();
			_target = (ItemsDatabaseAsset)target;

			var settings = _settings.Get("Items");
			foreach (var entry in settings.Sheets) {
				if (entry.Name == "Items")
					_downloaders.Add(new ItemsDownloader(settings.SpreadsheetId, entry, result => _target.All = new List<ItemData>(result)));
				else if (entry.Name == "Weapons")
					_downloaders.Add(new WeaponsDownloader(settings.SpreadsheetId, entry, result => _target.Weapons = new List<WeaponData>(result)));
				else if (entry.Name == "ContainerSettings")
					_downloaders.Add(new ContainerSettingsDownloader(settings.SpreadsheetId, entry, result => _target.ContainerSettings = new List<ContainerSettingsData>(result)));
			}
		}
	}
}