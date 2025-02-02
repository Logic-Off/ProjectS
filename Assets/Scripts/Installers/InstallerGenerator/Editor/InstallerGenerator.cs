using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Installers {
	/// <summary>
	///   Author: Andrey Abramkin
	/// </summary>
	public partial class InstallerGenerator {
		private const string SaveKeyPath = "Ecs.InstallerGenerator.Path";

		public static void Generate() {
			var installers = Find();
			foreach (var (installerName, entries) in installers) {
				Debug.Log(installerName);
				GenerateFile(installerName, entries);
			}

			var path = EditorPrefs.GetString(SaveKeyPath, "Scripts/Installers/");
			foreach (var file in _files)
				SaveToFile(file.Value, $"{file.Key}Installer.cs", path);
			Debug.Log("End generate default installers");
			AssetDatabase.Refresh();
			_files.Clear();
		}

		private static Dictionary<string, List<Entry>> Find() {
			var dictionary = new Dictionary<string, List<Entry>>();
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (var assembly in assemblies)
			foreach (var type in assembly.GetTypes()) {
				var attribute = type.GetCustomAttribute<InstallAttribute>();
				if (attribute == null)
					continue;
				if (!dictionary.ContainsKey(attribute.Name))
					dictionary.Add(attribute.Name, new List<Entry>());

				// Можно в целом сделать массив имен и кидать в несколько инсталлеров
				dictionary[attribute.Name].Add(new Entry(type, attribute));
			}

			foreach (var (_, entries) in dictionary)
				entries.Sort((a, b) => a.Attribute.Order.CompareTo(b.Attribute.Order));

			return dictionary;
		}

		private static void SaveToFile(string text, string filename, string path) {
			var filepath = $"{Application.dataPath}/{path}/{filename}";
			if (File.Exists(filepath))
				File.Delete(filepath);
			File.WriteAllText(filepath, text);
		}

		public sealed class Entry {
			public InstallAttribute Attribute;
			public Type Type;

			public Entry(Type type, InstallAttribute attribute) {
				Type = type;
				Attribute = attribute;
			}
		}
	}
}