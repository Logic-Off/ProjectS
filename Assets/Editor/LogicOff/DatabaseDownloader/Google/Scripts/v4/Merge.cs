using System;

namespace LogicOff.DatabaseDownloader.Google {
	[Serializable]
	public class Merge {
		public int SheetId;
		public int StartRowIndex;
		public int EndRowIndex;
		public int StartColumnIndex;
		public int EndColumnIndex;
	}
}