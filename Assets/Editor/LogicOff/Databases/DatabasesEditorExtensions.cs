using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LogicOff.Databases {
	/// <summary>
	/// </summary>
	public static class DatabasesEditorExtensions {
		public static Dictionary<string, Object> LoadDatabase(string databasePath) {
			var dictionary = new Dictionary<string, Object>();
			var database = LoadDatabase<ScriptableObject>(databasePath);
			dictionary.Add(database.name, database);
			return dictionary;
		}

		public static Dictionary<string, Object> LoadDatabases<T>(
			string databasePath,
			SearchOption searchOption = SearchOption.TopDirectoryOnly
		) where T : ScriptableObject {
			var dictionary = new Dictionary<string, Object>();
			var files = Directory.GetFiles(databasePath, $"*.asset", searchOption);
			foreach (var path in files) {
				var database = LoadDatabase<T>(path);
				if (!(database is T))
					continue;
				var name = !dictionary.ContainsKey(database.name) ? database.name : $"{Random.Range(0, 1000)}_{database.name}";
				dictionary.Add(name, database);
			}

			return dictionary;
		}

		public static Object LoadDatabase<T>(string databasePath) where T : Object
			=> AssetDatabase.LoadAssetAtPath<T>(databasePath);
	}
}