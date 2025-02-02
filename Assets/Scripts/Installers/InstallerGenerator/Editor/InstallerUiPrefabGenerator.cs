using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Ui;
using UnityEditor;
using UnityEngine;

namespace Installers {
	public partial class InstallerUiPrefabGenerator {
		private const string SaveKeyPath = "Ecs.InstallerGenerator.Path";

		public static void Generate() {
			var installers = Find();
			foreach (var (installerName, entries) in installers) {
				Debug.Log(installerName);
				GenerateFile(installerName, entries);
			}

			var path = EditorPrefs.GetString(SaveKeyPath, "Ecs/Installers/");
			foreach (var file in _files)
				SaveToFile(file.Value, $"{file.Key}UiPrefabsInstaller.cs", path);
			Debug.Log("End generate ui prefabs installers");
			AssetDatabase.Refresh();
			_files.Clear();
		}

		private static Dictionary<string, List<Entry>> Find() {
			var dictionary = new Dictionary<string, List<Entry>>();
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (var assembly in assemblies)
			foreach (var type in assembly.GetTypes()) {
				if (type.BaseType == null)
					continue;

				if (!type.BaseType.GetInterfaces().Contains(typeof(IPanel)))
					continue;

				var attribute = type.GetCustomAttribute<InstallUiPrefabAttribute>();
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
			public InstallUiPrefabAttribute Attribute;
			public Type BuilderType;

			public Entry(Type builderType, InstallUiPrefabAttribute attribute) {
				BuilderType = builderType;
				Attribute = attribute;
			}
		}
	}
}