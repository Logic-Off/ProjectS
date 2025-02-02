using System.Collections.Generic;
using System.IO;
using Common;
using LogicOff.Extensions;
using UnityEditor;
using UnityEngine;

namespace LogicOff.Toolbar {
	/// <summary>
	/// Store the editor preferences for Toolbar.
	/// </summary>
	[FilePath("UserSettings/ToolbarPreferences.asset", FilePathAttribute.Location.ProjectFolder)]
	public sealed class ToolbarPreferences : ScriptableSingleton<ToolbarPreferences> {
		public List<ToolbarSettingsEntry> DefaultEntries = new();

		public List<ToolbarSettingsEntry> DefaultEntriesV1 = new() {
			new ToolbarSettingsEntry() {
				Name = "Open IDE",
				IsVisible = false,
				Type = EToolbarObjectType.Button,
				ZoneAlign = EToolbarZoneAlign.Left,
				Assembly = "Assembly-CSharp-Editor",
				ClassFullName = "LogicOff.Toolbar.Extensions",
				Method = "OnOpenIDE",
				Style = "ToolbarDefaultButton",
				Options = new()
			},
			new ToolbarSettingsEntry() {
				Name = "Generate",
				IsVisible = false,
				Type = EToolbarObjectType.Dropdown,
				ZoneAlign = EToolbarZoneAlign.Left,
				Assembly = "Assembly-CSharp-Editor",
				ClassFullName = "LogicOff.Toolbar.Extensions",
				Method = "OnGenerate",
				Style = "ToolbarDefaultButton",
				Options = new() {
					"Events",
					"Installers",
					"ECS",
				}
			},
			new ToolbarSettingsEntry() {
				Name = "Databases",
				IsVisible = true,
				Type = EToolbarObjectType.Button,
				ZoneAlign = EToolbarZoneAlign.Left,
				Assembly = "Assembly-CSharp-Editor",
				ClassFullName = "LogicOff.Toolbar.Extensions",
				Method = "OnOpenDatabases",
				Style = "ToolbarDefaultButton",
				Options = new()
			},
			new ToolbarSettingsEntry() {
				Name = "Save",
				IsVisible = true,
				Type = EToolbarObjectType.Dropdown,
				ZoneAlign = EToolbarZoneAlign.Left,
				Assembly = "Assembly-CSharp-Editor",
				ClassFullName = "LogicOff.Toolbar.Extensions",
				Method = "OnSave",
				Style = "ToolbarDefaultButton",
				Options = new() {
					"Load/1",
					"Load/2",
					"Load/3",
					"Save/1",
					"Save/2",
					"Save/3",
					"Delete",
				}
			},
			new ToolbarSettingsEntry() {
				Name = "Load Scene",
				IsVisible = true,
				Type = EToolbarObjectType.Dropdown,
				ZoneAlign = EToolbarZoneAlign.Right,
				Assembly = "Assembly-CSharp-Editor",
				ClassFullName = "LogicOff.Toolbar.Extensions",
				Method = "OnLoadScene",
				Style = "ToolbarDefaultButton",
				Options = new() {
					"Base",
				}
			},
			new ToolbarSettingsEntry() {
				Name = "Set Scene",
				IsVisible = true,
				Type = EToolbarObjectType.Dropdown,
				ZoneAlign = EToolbarZoneAlign.Right,
				Assembly = "Assembly-CSharp-Editor",
				ClassFullName = "LogicOff.Toolbar.Extensions",
				Method = "OnSetFirstScene",
				Style = "ToolbarDefaultButton",
				Options = new() {
					"Base",
				}
			},
			new ToolbarSettingsEntry() {
				Name = "AutoStartScene",
				IsVisible = true,
				Type = EToolbarObjectType.Toggle,
				ZoneAlign = EToolbarZoneAlign.Right,
				Assembly = "Assembly-CSharp-Editor",
				ClassFullName = "LogicOff.Toolbar.Extensions",
				Method = "OnChangeAutoStartScene",
				Style = "ToolbarDefaultButton",
				Options = new() {
					"EditorSetStartScene"
				}
			},
		};

		public List<ToolbarSettingsEntry> Entries = new();

		private void OnEnable() {
			DefaultEntries = new List<ToolbarSettingsEntry>(DefaultEntriesV1);
			if (Entries.Count == 0)
				Entries = new List<ToolbarSettingsEntry>(DefaultEntriesV1);
		}

		void OnDisable() => Save();

		public void Save() => Save(true);

		public void OnCollectScenes() {
			var allScenes = Directory.GetFiles(Application.dataPath + "/Scenes", "*.unity", SearchOption.AllDirectories);
			List<string> scenes = new();
			for (var i = 0; i < allScenes.Length; i++) {
				var sceneName = allScenes[i].Split("\\").Last();
				if (!sceneName.Contains("Manager") && !sceneName.Contains("StartScene"))
					scenes.Add(sceneName.Split(".").First());
			}

			foreach (var entry in Entries) {
				if (entry.Name == "Load Scene" || entry.Name == "Set Scene") {
					entry.Options.Clear();
					entry.Options.AddRange(scenes);
				}
			}
		}

		internal SerializedObject GetSerializedObject() => new(this);
	}
}