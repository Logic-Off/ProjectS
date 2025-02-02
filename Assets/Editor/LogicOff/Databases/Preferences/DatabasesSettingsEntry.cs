using System;
using System.IO;
using Object = UnityEngine.Object;

namespace LogicOff.Databases {
	[Serializable]
	public struct DatabasesSettingsEntry {
		public string Name;
		public Object Target;
		public EDatabasesSettingsEntryType Type;
		public SearchOption SearchOption;
		public string Domain;
		public string ClassFullName;
	}
}