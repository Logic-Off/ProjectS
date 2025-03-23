using System;
using System.Collections.Generic;
using UnityEngine;

namespace LogicOff.DatabaseDownloader {
	[CreateAssetMenu(menuName = "Editor/LogicOff/DatabaseDownloader/DownloadDatabasesSettings", fileName = "DownloadDatabasesSettings")]
	public sealed class DatabaseSheetsSettings : ScriptableObject {
		public List<Entry> All;

		public Entry Get(string name) {
			foreach (var entry in All)
				if (entry.Name == name)
					return entry;
			throw new Exception($"Настройки базы данных '{name}' не найдены");
		}

		[Serializable]
		public struct Entry {
			public string Name;
			public string SpreadsheetId;
			public List<SheetEntry> Sheets;
		}
	}
}