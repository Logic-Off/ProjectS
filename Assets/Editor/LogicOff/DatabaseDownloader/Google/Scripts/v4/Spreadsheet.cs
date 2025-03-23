namespace LogicOff.DatabaseDownloader.Google {
	/// <summary>
	/// Search class for accessing a database
	/// </summary>
	public class Spreadsheet {
		public readonly string SheetId = "";
		public readonly string WorksheetName = "Sheet1";

		public readonly string StartCell = "A1";
		public readonly string EndCell = "Z100";

		public readonly string TitleColumn = "A";
		public readonly int TitleRow = 1;

		public Spreadsheet(string sheetId, string worksheetName) {
			SheetId = sheetId;
			WorksheetName = worksheetName;
		}

		public Spreadsheet(string sheetId, string worksheetName, string startCell) {
			SheetId = sheetId;
			WorksheetName = worksheetName;
			StartCell = startCell;
		}

		public Spreadsheet(string sheetId, string worksheetName, string startCell, string endCell) {
			SheetId = sheetId;
			WorksheetName = worksheetName;
			StartCell = startCell;
			EndCell = endCell;
		}

		public Spreadsheet(string sheetId, string worksheetName, string startCell, string endCell, string titleColumn, int titleRow) {
			SheetId = sheetId;
			WorksheetName = worksheetName;
			StartCell = startCell;
			EndCell = endCell;
			TitleColumn = titleColumn;
			TitleRow = titleRow;
		}
	}
}