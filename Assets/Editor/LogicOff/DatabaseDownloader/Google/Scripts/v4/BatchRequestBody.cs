using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace LogicOff.DatabaseDownloader.Google {
	[Serializable]
	public class BatchRequestBody {
		public ValueInputOption ValueInputOption = ValueInputOption.UserEntered;
		public List<ValueRange> Data = new();

		public void Add(ValueRange data) => Data.Add(data);

		public void Send(string spreadsheetId, string worksheet, UnityAction callback)
			=> SpreadsheetManager.WriteBatch(new Spreadsheet(spreadsheetId, worksheet), this, callback);
	}
}