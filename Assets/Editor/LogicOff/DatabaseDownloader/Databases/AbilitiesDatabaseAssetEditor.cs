using System.Collections.Generic;
using Ecs.Ability;
using Ecs.Character;
using UnityEditor;

namespace LogicOff.DatabaseDownloader {
	[CustomEditor(typeof(AbilitiesDatabaseAsset))]
	public sealed class AbilitiesDatabaseAssetEditor : ADatabaseDownloader {
		private AbilitiesDatabaseAsset _target;

		protected override void OnEnable() {
			base.OnEnable();
			_target = (AbilitiesDatabaseAsset)target;

			var settings = _settings.Get("Ability");
			foreach (var entry in settings.Sheets) {
				if (entry.Name == "Abilities")
					_downloaders.Add(new AbilitiesDownloader(settings.SpreadsheetId, entry, result => _target.Abilities = new List<AbilityData>(result)));
			}
		}
	}
}