using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TinyJSON;
using UnityEngine;
using UnityEngine.Networking;

namespace LogicOff.DatabaseDownloader.Google {
	/// <summary>
	/// Partial class for the spreadsheet manager to handle all Public functions
	/// </summary>
	public partial class SpreadsheetManager {
		private static GoogleSheetsConfig _config;

		/// <summary>
		/// Reference to the config for access to the auth details
		/// </summary>
		public static GoogleSheetsConfig Config {
			get {
				if (_config == null)
					_config = (GoogleSheetsConfig)Resources.Load("GoogleSheetsConfig");

				return _config;
			}
			set { _config = value; }
		}

		/// <summary>
		/// Read a public accessable spreadsheet
		/// </summary>
		/// <param name="searchDetails"></param>
		/// <param name="callback">event that will fire after reading is complete</param>
		public static async Task<List<List<string>>> ReadPublicSpreadsheet(Spreadsheet searchDetails) {
			if (string.IsNullOrEmpty(Config.API_Key)) {
				Debug.Log("Missing API Key, please enter this in the confie settings");
				return null;
			}

			var sb = new StringBuilder();
			sb.Append("https://sheets.googleapis.com/v4/spreadsheets");
			sb.Append("/" + searchDetails.SheetId);
			sb.Append("/values");
			sb.Append("/" + searchDetails.WorksheetName + "!" + searchDetails.StartCell + ":" + searchDetails.EndCell);
			sb.Append("?key=" + Config.API_Key);

			var request = UnityWebRequest.Get(sb.ToString());
			request.timeout = 10;
			request.SendWebRequest();

			while (!request.isDone)
				await Task.Delay(10);

			Debug.Log(request.downloadHandler.text);
			var rawData = JSON.Load(request.downloadHandler.text).Make<ValueRange>();
			return rawData.values;
		}
	}
}