using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LogicOff.DatabaseDownloader.Google {
	[Serializable]
	public class SpreadsheetModel {
		/// <summary>
		///     All the cells that the spreadsheet loaded
		///     Index is Cell ID IE "A2"
		/// </summary>
		/// <returns></returns>
		public Dictionary<string, SpreadsheetCell> Cells = new();

		public SecondaryKeyDictionary<string, List<SpreadsheetCell>> columns = new();

		public SecondaryKeyDictionary<int, string, List<SpreadsheetCell>> rows = new();

		public SpreadsheetModel(SpreadsheetResponse data, string titleColumn, int titleRow) {
			var startColumn = Regex.Replace(data.StartCell(), "[^a-zA-Z]", "");
			var startRow = int.Parse(Regex.Replace(data.StartCell(), "[^0-9]", ""));

			var startColumnAsInt = GoogleSheetsToUnityUtilities.NumberFromExcelColumn(startColumn);
			var currentRow = startRow;

			var mergeCellRedirect = new Dictionary<string, string>();
			if (data.SheetInfo != null) {
				foreach (var merge in data.SheetInfo.merges) {
					var cell = GoogleSheetsToUnityUtilities.ExcelColumnFromNumber(merge.StartColumnIndex + 1) + (merge.StartRowIndex + 1);

					for (var r = merge.StartRowIndex; r < merge.EndRowIndex; r++) {
						for (var c = merge.StartColumnIndex; c < merge.EndColumnIndex; c++) {
							var mergeCell = GoogleSheetsToUnityUtilities.ExcelColumnFromNumber(c + 1) + (r + 1);
							mergeCellRedirect.Add(mergeCell, cell);
						}
					}
				}
			}

			foreach (List<string> dataValue in data.ValueRange.values) {
				var currentColumn = startColumnAsInt;

				foreach (var entry in dataValue) {
					var realColumn = GoogleSheetsToUnityUtilities.ExcelColumnFromNumber(currentColumn);
					var cellID = realColumn + currentRow;

					SpreadsheetCell cell = null;
					if (mergeCellRedirect.ContainsKey(cellID) && Cells.ContainsKey(mergeCellRedirect[cellID])) {
						cell = Cells[mergeCellRedirect[cellID]];
					} else {
						cell = new SpreadsheetCell(entry, realColumn, currentRow);

						//check the title row and column exist, if not create them
						if (!rows.ContainsKey(currentRow)) {
							rows.Add(currentRow, new List<SpreadsheetCell>());
						}

						if (!columns.ContainsPrimaryKey(realColumn)) {
							columns.Add(realColumn, new List<SpreadsheetCell>());
						}

						rows[currentRow].Add(cell);
						columns[realColumn].Add(cell);

						//build a series of seconard keys for the rows and columns
						if (realColumn == titleColumn) {
							rows.LinkSecondaryKey(currentRow, cell.Value);
						}

						if (currentRow == titleRow) {
							columns.LinkSecondaryKey(realColumn, cell.Value);
						}
					}

					Cells.Add(cellID, cell);

					currentColumn++;
				}

				currentRow++;
			}

			//build the column and row string Id's from titles
			foreach (SpreadsheetCell cell in Cells.Values) {
				cell.ColumnId = Cells[cell.Column() + titleRow].Value;
				cell.RowId = Cells[titleColumn + cell.Row()].Value;
			}

			//build all links to row and columns for cells that are handled by merged title fields.
			foreach (SpreadsheetCell cell in Cells.Values) {
				foreach (KeyValuePair<string, SpreadsheetCell> cell2 in Cells) {
					if (cell.ColumnId == cell2.Value.ColumnId && cell.RowId == cell2.Value.RowId) {
						if (!cell.TitleConnectedCells.Contains(cell2.Key)) {
							cell.TitleConnectedCells.Add(cell2.Key);
						}
					}
				}
			}
		}

		public SpreadsheetCell this[string cellRef] { get { return Cells[cellRef]; } }

		public SpreadsheetCell this[string rowId, string columnId] {
			get {
				string columnIndex = columns.secondaryKeyLink[columnId];
				int rowIndex = rows.secondaryKeyLink[rowId];

				return Cells[columnIndex + rowIndex];
			}
		}

		public List<SpreadsheetCell> this[string rowID, string columnID, bool mergedCells] {
			get {
				string columnIndex = columns.secondaryKeyLink[columnID];
				int rowIndex = rows.secondaryKeyLink[rowID];
				List<string> actualCells = Cells[columnIndex + rowIndex].TitleConnectedCells;

				List<SpreadsheetCell> returnCells = new List<SpreadsheetCell>();
				foreach (string s in actualCells) {
					returnCells.Add(Cells[s]);
				}

				return returnCells;
			}
		}
	}
}