using System.Collections.Generic;
using UnityEditor;

namespace LogicOff.Databases {
	/// <summary>
	/// Store the editor preferences for Databases.
	/// </summary>
	[FilePath("DatabasesPreferences.asset", FilePathAttribute.Location.PreferencesFolder)]
	public sealed class DatabasesPreferences : ScriptableSingleton<DatabasesPreferences> {
		// Стандартные базовые настройки
		public List<DatabasesSettingsEntry> DefaultEntries = new() { };

		public List<DatabasesSettingsEntry> Entries = new();

		private void OnEnable() {
			if (Entries.Count == 0)
				Entries = new List<DatabasesSettingsEntry>(DefaultEntries);
		}

		void OnDisable() => Save();

		public void Save() => Save(true);

		internal SerializedObject GetSerializedObject() => new(this);
	}
}