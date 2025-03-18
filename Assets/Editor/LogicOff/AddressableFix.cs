using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace _Trash {
	[InitializeOnLoad]
	public class AddressableFix {
		static AddressableFix() {
			var cSharpFiles = Directory.GetFiles(Application.dataPath.Replace("Assets", "Library/PackageCache"), "*.cs", SearchOption.AllDirectories);
			var filePath = string.Empty;
			foreach (var file in cSharpFiles) {
				if (!file.Contains("AddressableAssetSettings.cs")) {
					continue;
				}

				filePath = file;
				break;
			}

			var lines = File.ReadAllLines(filePath);
			for (var i = 0; i < lines.Length; i++) {
				var line = lines[i];
				if (!line.Contains("m_GroupAssets.Sort((a, b) => string.CompareOrdinal(a?.Guid, b?.Guid));"))
					continue;
				lines[i] = "            m_GroupAssets.Sort((a, b) => string.CompareOrdinal(a?.Name, b?.Name));";
				D.Error("[AddressableFix.Line]", i);
			}

			File.WriteAllLines(filePath, lines);

			var settings = AddressableAssetSettingsDefaultObject.GetSettings(false);
			settings?.OnBeforeSerialize();
		}
	}
}