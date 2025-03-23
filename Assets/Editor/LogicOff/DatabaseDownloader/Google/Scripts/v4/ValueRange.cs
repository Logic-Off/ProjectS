using System;
using System.Collections.Generic;

namespace LogicOff.DatabaseDownloader.Google {
	[Serializable]
	public class ValueRange {
		public string range = "";
		public string majorDimension;
		public List<List<string>> values = new();

		public ValueRange() { }

		/// <summary>
		/// Used to create a spreadshhet that can be returned to google sheets
		/// </summary>
		/// <param name="data"></param>
		public ValueRange(List<List<string>> data) => values = data;

		/// <summary>
		/// Used to create a spreadshhet that can be returned to google sheets
		/// </summary>
		/// <param name="data"></param>
		public ValueRange(List<string> data) => values.Add(data);

		public ValueRange(string data) => values.Add(new List<string>() { data });

		public void Add(List<string> data) => values.Add(data);
	}
}