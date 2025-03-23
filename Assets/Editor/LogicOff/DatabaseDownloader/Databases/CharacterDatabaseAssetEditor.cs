using System.Collections.Generic;
using Ecs.Character;
using UnityEditor;

namespace LogicOff.DatabaseDownloader {
	[CustomEditor(typeof(CharacterDatabaseAsset))]
	public sealed class CharacterDatabaseAssetEditor : ADatabaseDownloader {
		private CharacterDatabaseAsset _target;

		protected override void OnEnable() {
			base.OnEnable();
			_target = (CharacterDatabaseAsset)target;

			var settings = _settings.Get("Character");
			foreach (var entry in settings.Sheets) {
				if (entry.Name == "Characters")
					_downloaders.Add(new CharactersDownloader(settings.SpreadsheetId, entry, result => _target.Characters = new List<CharacterData>(result)));
				else if (entry.Name == "Buffs")
					_downloaders.Add(new BuffsDownloader(settings.SpreadsheetId, entry, result => _target.Buffs = new List<BuffData>(result)));
			}
		}
	}
}