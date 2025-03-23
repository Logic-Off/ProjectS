using System;

namespace LogicOff.DatabaseDownloader.Google {
	[Serializable]
	public class SpreadsheetResponse {
		public ValueRange ValueRange;
		internal Sheet SheetInfo = null;

		public SpreadsheetResponse() { }

		public SpreadsheetResponse(ValueRange data) => ValueRange = data;

		public string WorkSheet() => ValueRange.range.Substring(0, ValueRange.range.IndexOf("!") - 1);

		public string StartCell() {
			var start = ValueRange.range.IndexOf("!") + 1;
			var end = ValueRange.range.IndexOf(":", start);
			return ValueRange.range.Substring(start, end - start);
		}

		public string EndCell() => ValueRange.range.Substring(ValueRange.range.IndexOf(":") + 1);
	}
}