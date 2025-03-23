using System;

namespace LogicOff.DatabaseDownloader {
	[Serializable]
	public class SheetEntry {
		public string Name;
		public string StartCell = "A3";
		public string EndCell = "AZ1000";
		public string TitleColumn = "A";
		public int TitleRow = 1;
	}
}