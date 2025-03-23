using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace LogicOff.DatabaseDownloader.Google.Editor {
	public sealed class GoogleSheetsToUnityEditorWindow : EditorWindow {
		private GoogleSheetsConfig _config;
		private bool _showSecret = false;
		private int _tabID = 0;

		[MenuItem("Window/Logic Off/Google Sheets/Open Config")]
		private static void Open() {
			var window = GetWindow<GoogleSheetsToUnityEditorWindow>("Google Sheets Config");
			ServicePointManager.ServerCertificateValidationCallback = Validator;

			window.Init();
		}

		public static bool Validator(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) => true;

		public void Init() => _config = (GoogleSheetsConfig)Resources.Load("GoogleSheetsConfig");

		void OnGUI() {
			_tabID = GUILayout.Toolbar(_tabID, new string[] { "Private", "Public" });

			if (_config == null) {
				Debug.LogError("Error: no config file");
				return;
			}

			switch (_tabID) {
				case 0: {
					_config.CLIENT_ID = EditorGUILayout.TextField("Client ID", _config.CLIENT_ID);

					GUILayout.BeginHorizontal();
					if (_showSecret)
						_config.CLIENT_SECRET = EditorGUILayout.TextField("Client Secret Code", _config.CLIENT_SECRET);
					else
						_config.CLIENT_SECRET = EditorGUILayout.PasswordField("Client Secret Code", _config.CLIENT_SECRET);

					_showSecret = GUILayout.Toggle(_showSecret, "Show");
					GUILayout.EndHorizontal();

					_config.PORT = EditorGUILayout.IntField("Port number", _config.PORT);

					if (GUILayout.Button("Build Connection"))
						GoogleAuthrisationHelper.BuildHttpListener();
					if(GUILayout.Button("If problem auth"))
						Application.OpenURL("https://developers.google.com/oauthplayground/");

					break;
				}
				case 1: {
					_config.API_Key = EditorGUILayout.TextField("API Key", _config.API_Key);
					break;
				}
			}

			EditorUtility.SetDirty(_config);
		}
	}
}