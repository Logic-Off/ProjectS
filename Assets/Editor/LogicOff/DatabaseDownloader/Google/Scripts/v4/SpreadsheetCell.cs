using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace LogicOff.DatabaseDownloader.Google {
	[Serializable]
	public class SpreadsheetCell {
		private string _column = string.Empty;
		public string ColumnId = string.Empty;
		private int _row = -1;
		public string RowId = string.Empty;
		public string Value;
		internal List<string> TitleConnectedCells = new();

		public SpreadsheetCell(string value, string column, int row) {
			Value = value;
			_column = column;
			_row = row;
		}

		public SpreadsheetCell(string value) => Value = value;

		public string Column() => _column;

		public int Row() => _row;

		public string CellRef() => _column + _row;

		//TODO: store the sheetId and worksheet in the spreadsheet so dont have to pass these through
		internal void UpdateCellValue(string sheetID, string worksheet, string value, UnityAction callback = null) {
			Value = value;
			List<string> list = new List<string>();
			list.Add(value);
			SpreadsheetManager.Write(new Spreadsheet(sheetID, worksheet, CellRef()), new ValueRange(list), callback);
		}

		//TODO: store the sheetId and worksheet in the spreadsheet so dont have to pass these through
		internal ValueRange AddCellToBatchUpdate(string sheetID, string worksheet, string value) {
			Value = value;
			List<string> list = new List<string>();
			list.Add(value);
			ValueRange data = new ValueRange(list);
			data.range = CellRef();
			return data;
		}
	}
}