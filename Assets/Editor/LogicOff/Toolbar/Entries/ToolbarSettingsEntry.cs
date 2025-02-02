using System;
using System.Collections.Generic;
using UnityEngine;

namespace LogicOff.Toolbar {
	[Serializable]
	public class ToolbarSettingsEntry {
		public string Name;
		public bool IsVisible;
		public EToolbarObjectType Type;
		public EToolbarZoneAlign ZoneAlign;

		public string Assembly = "Assembly-CSharp-Editor";
		public string ClassFullName = "LogicOff.Toolbar.Extensions";
		[Header("Статический метод")] public string Method;
		[Header("Стиль кнопки")] public string Style;

		public List<string> Options = new();
	}
}