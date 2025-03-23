using System;
using UnityEngine;

namespace LogicOff.DatabaseDownloader.Google {
	[CreateAssetMenu(menuName = "Editor/LogicOff/DatabaseDownloader/GoogleSheetsConfig", fileName = "GoogleSheetsConfig")]
	public class GoogleSheetsConfig : ScriptableObject {
		public string CLIENT_ID = "";
		public string CLIENT_SECRET = "";
		public string ACCESS_TOKEN = "";

		[HideInInspector] public string REFRESH_TOKEN;

		public string API_Key = "";

		public int PORT;

		public GoogleDataResponse gdr;
	}

	[System.Serializable]
	public class GoogleDataResponse {
		public string access_token = "";
		public string refresh_token = "";
		public string token_type = "";
		public int expires_in = 0; //just a place holder to work the the json and caculate the next refresh time
		public DateTime nextRefreshTime;
	}
}