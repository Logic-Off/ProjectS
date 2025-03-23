using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace LogicOff.DatabaseDownloader.Google {
	/// <summary>
	/// Partial class for the spreadsheet manager to handle all private functions
	/// </summary>
	public partial class SpreadsheetManager {
		/// <summary>
		/// Chekcs for a valid token and if its out of date attempt to refresh it
		/// </summary>
		/// <returns></returns>
		static async Task CheckForRefreshToken() {
			var coroutine = EditorCoroutineRunner.StartCoroutine(GoogleAuthrisationHelper.CheckForRefreshOfToken());
			while (!coroutine.HasFinished)
				await Task.Delay(100);
		}

		/// <summary>
		/// Reads information from a spreadsheet
		/// </summary>
		/// <param name="search"></param>
		/// <param name="callback"></param>
		/// <param name="containsMergedCells"> does the spreadsheet contain merged cells, will attempt to group these by titles</param>
		public static async Task<List<List<string>>> Read(Spreadsheet search, bool containsMergedCells = false) {
			StringBuilder sb = new StringBuilder();
			sb.Append("https://sheets.googleapis.com/v4/spreadsheets");
			sb.Append("/" + search.SheetId);
			sb.Append("/values");
			sb.Append("/" + search.WorksheetName + "!" + search.StartCell + ":" + search.EndCell);
			sb.Append("?access_token=" + SpreadsheetManager.Config.gdr.access_token);

			UnityWebRequest request = UnityWebRequest.Get(sb.ToString());

			return await Read(request, search, containsMergedCells);
		}

		/// <summary>
		/// Reads the spread sheet and callback with the results
		/// </summary>
		/// <param name="request"></param>
		/// <param name="search"></param>
		/// <param name="containsMergedCells"></param>
		/// <param name="callback"></param>
		/// <returns></returns>
		static async Task<List<List<string>>> Read(UnityWebRequest request, Spreadsheet search, bool containsMergedCells) {
			await CheckForRefreshToken();

			using (request) {
				request.SendWebRequest();
				while (!request.isDone)
					await Task.Delay(10);

				if (string.IsNullOrEmpty(request.downloadHandler.text) || request.downloadHandler.text == "{}") {
					Debug.LogWarning("Unable to Retreive data from google sheets");
					return null;
				}

				ValueRange rawData = JSON.Load(request.downloadHandler.text).Make<ValueRange>();
				Debug.Log(request.downloadHandler.text);
				return rawData.values;
				SpreadsheetResponse response = new SpreadsheetResponse(rawData);

				//if it contains merged cells then process a second set of json data to know what these cells are
				if (containsMergedCells) {
					StringBuilder sb = new StringBuilder();
					sb.Append("https://sheets.googleapis.com/v4/spreadsheets");
					sb.Append("/" + search.SheetId);
					sb.Append("?access_token=" + SpreadsheetManager.Config.gdr.access_token);

					UnityWebRequest request2 = UnityWebRequest.Get(sb.ToString());

					request2.SendWebRequest();
					while (!request2.isDone)
						await Task.Delay(100);

					SheetsRootObject root = JSON.Load(request2.downloadHandler.text).Make<SheetsRootObject>();
					response.SheetInfo = root.sheets.FirstOrDefault(x => x.properties.Title == search.WorksheetName);
				}

				return rawData.values;
			}
		}

		/// <summary>
		/// Updates just the cell pased in as the startCell value of the search parameters
		/// </summary>
		/// <param name="search"></param>
		/// <param name="inputData"></param>
		/// <param name="callback"></param>
		public static void Write(Spreadsheet search, string inputData, UnityAction callback) => Write(search, new ValueRange(inputData), callback);

		/// <summary>
		/// Writes data to a spreadsheet
		/// </summary>
		/// <param name="search"></param>
		/// <param name="inputData"></param>
		/// <param name="callback"></param>
		public static async Task Write(Spreadsheet search, ValueRange inputData, UnityAction callback) {
			StringBuilder sb = new StringBuilder();
			sb.Append("https://sheets.googleapis.com/v4/spreadsheets");
			sb.Append("/" + search.SheetId);
			sb.Append("/values");
			sb.Append("/" + search.WorksheetName + "!" + search.StartCell + ":" + search.EndCell);
			sb.Append("?valueInputOption=USER_ENTERED");
			sb.Append("&access_token=" + SpreadsheetManager.Config.gdr.access_token);

			string json = JSON.Dump(inputData, EncodeOptions.NoTypeHints);
			byte[] bodyRaw = new UTF8Encoding().GetBytes(json);

			UnityWebRequest request = UnityWebRequest.Put(sb.ToString(), bodyRaw);

			Write(request, callback);
		}

		static async Task Write(UnityWebRequest request, UnityAction callback) {
			await CheckForRefreshToken();

			using (request) {
				request.SendWebRequest();
				while (!request.isDone)
					await Task.Delay(100);

				if (callback != null)
					callback();
			}
		}

		/// <summary>
		/// Writes a batch update to a spreadsheet
		/// </summary>
		/// <param name="search"></param>
		/// <param name="requestData"></param>
		/// <param name="callback"></param>
		public static void WriteBatch(Spreadsheet search, BatchRequestBody requestData, UnityAction callback) {
			StringBuilder sb = new StringBuilder();
			sb.Append("https://sheets.googleapis.com/v4/spreadsheets");
			sb.Append("/" + search.SheetId);
			sb.Append("/values:batchUpdate");
			sb.Append("?access_token=" + SpreadsheetManager.Config.gdr.access_token);

			string json = JSON.Dump(requestData, EncodeOptions.NoTypeHints);
			UnityWebRequest request = UnityWebRequest.PostWwwForm(sb.ToString(), "");
			byte[] bodyRaw = new UTF8Encoding().GetBytes(json);
			request.uploadHandler = new UploadHandlerRaw(bodyRaw);
			request.downloadHandler = new DownloadHandlerBuffer();
			WriteBatch(request, callback);
		}

		static async Task WriteBatch(UnityWebRequest request, UnityAction callback) {
			await CheckForRefreshToken();

			using (request) {
				request.SendWebRequest();
				while (!request.isDone)
					await Task.Delay(100);

				if (callback != null) {
					callback();
				}
			}
		}

		/// <summary>
		/// Adds the data to the next avaiable space to write it after the startcell
		/// </summary>
		/// <param name="search"></param>
		/// <param name="inputData"></param>
		/// <param name="callback"></param>
		public static void Append(Spreadsheet search, ValueRange inputData, UnityAction callback) {
			StringBuilder sb = new StringBuilder();
			sb.Append("https://sheets.googleapis.com/v4/spreadsheets");
			sb.Append("/" + search.SheetId);
			sb.Append("/values");
			sb.Append("/" + search.WorksheetName + "!" + search.StartCell);
			sb.Append(":append");
			sb.Append("?valueInputOption=USER_ENTERED");
			sb.Append("&access_token=" + SpreadsheetManager.Config.gdr.access_token);

			var json = JSON.Dump(inputData, EncodeOptions.NoTypeHints);

			var request = UnityWebRequest.PostWwwForm(sb.ToString(), "");

			//have to do this cause unitywebrequest post will nto accept json data corrently...
			var bodyRaw = new UTF8Encoding().GetBytes(json);
			request.uploadHandler = new UploadHandlerRaw(bodyRaw);
			request.downloadHandler = new DownloadHandlerBuffer();
			Append(request, callback);
		}

		private static async Task Append(UnityWebRequest request, UnityAction callback) {
			await CheckForRefreshToken();

			using (request) {
				request.SendWebRequest();
				while (!request.isDone)
					await Task.Delay(100);

				if (callback != null)
					callback();
			}
		}
	}
}