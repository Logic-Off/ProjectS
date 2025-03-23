using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
#if UNITY_EDITOR
#endif

namespace LogicOff.DatabaseDownloader.Google {
	public class GoogleAuthrisationHelper : MonoBehaviour {
		static string _authToken = "";

		static HttpListener _httpListener;

		static string
			_htmlResponseContent = "<h1>Google Sheets and Unity are now linked, you may close this window</h1>"; //message shown after connection has been set up

		private static object _notifyAuthTokenLock = new object();
		private static bool _shouldNotifyAuthTokenReceived = false;
		private static Action<string> _onComplete;

#if UNITY_EDITOR
		public static void BuildHttpListener() {
			if (_httpListener != null) {
				_httpListener.Abort();
				_httpListener = null;
			}

			_onComplete = null;

			string serverUrl = string.Format("http://127.0.0.1:{0}", SpreadsheetManager.Config.PORT);

			_httpListener = new HttpListener();
			_httpListener.Prefixes.Add(serverUrl + "/");
			_httpListener.Start();
			_httpListener.BeginGetContext(new AsyncCallback(ListenerCallback), _httpListener);

			_onComplete += GetAuthComplete;

			string request = "https://accounts.google.com/o/oauth2/v2/auth?";
			request += "client_id=" + Uri.EscapeDataString(SpreadsheetManager.Config.CLIENT_ID) + "&";
			request += "redirect_uri=" + Uri.EscapeDataString(serverUrl) + "&";
			request += "response_type=" + "code" + "&";
			request += "scope=" + Uri.EscapeDataString("https://www.googleapis.com/auth/spreadsheets") + "&";
			request += "access_type=" + "offline" + "&";
			request += "prompt=" + "consent" + "&";

			Application.OpenURL(request);
		}

		static void GetAuthComplete(string authToken) {
			string serverUrl = string.Format("http://127.0.0.1:{0}", SpreadsheetManager.Config.PORT);
			Debug.Log(authToken);
			Debug.Log("Auth Token = " + authToken);

			var f = new Dictionary<string, string>();
			f.Add("code", authToken);
			f.Add("redirect_uri", serverUrl);
			f.Add("client_id", SpreadsheetManager.Config.CLIENT_ID);
			f.Add("client_secret", SpreadsheetManager.Config.CLIENT_SECRET);
			f.Add("scope", "");
			f.Add("grant_type", "authorization_code");

			EditorCoroutineRunner.StartCoroutine(GetToken(f));
		}

		private static IEnumerator GetToken(Dictionary<string, string> f) {
			using (UnityWebRequest request = UnityWebRequest.Post("https://oauth2.googleapis.com/token", f)) {
				yield return request.SendWebRequest();

				SpreadsheetManager.Config.gdr = JsonUtility.FromJson<GoogleDataResponse>(request.downloadHandler.text);
				SpreadsheetManager.Config.gdr.nextRefreshTime = DateTime.Now.AddSeconds(SpreadsheetManager.Config.gdr.expires_in);
				EditorUtility.SetDirty(SpreadsheetManager.Config);
				AssetDatabase.SaveAssets();
			}
		}

		static void ListenerCallback(IAsyncResult result) {
			if (_httpListener != null) {
				try {
					HttpListenerContext context = _httpListener.EndGetContext(result);
					HandleListenerContextResponse(context);
					ProcessListenerContext(context);

					context.Response.Close();
					_httpListener.BeginGetContext(
						ListenerCallback,
						_httpListener
					); // EndGetContext above ends the async listener, so we need to start it up again to continue listening.
				} catch (ObjectDisposedException) {
					// Intentionally ignoring this exception because it will be thrown when we stop listening.
				} catch (Exception exception) {
					Debug.Log(exception.Message + " : " + exception.StackTrace); // Just in case...
				}
			}
		}

		static void ProcessListenerContext(HttpListenerContext context) {
			// Attempt to pull out the URI fragment as a part of the query string.
			string uriFragment = context.Request.QueryString["code"];
			if (uriFragment != null) {
				// If it worked, that means we're being passed the auth token from Instagram, so pull it out and notify that we received it.
				string authToken = uriFragment.Replace("access_token=", "");
				NotifyAuthTokenReceived(authToken);
			}
		}

		// GET /oauthplayground/?code=4/0ATx3LY6zayacCmk-OgzWKsOw_nFAmxOczM12UzZOOiUvimHqbXIexVw1xd39sx-LMiPdjw&scope=https://www.googleapis.com/auth/spreadsheets HTTP/1.1

		/// <summary>
		/// Child classes should call this once the auth token has been successfully retrieved.</summary>
		static void NotifyAuthTokenReceived(string authToken) {
			lock (_notifyAuthTokenLock) {
				// We're not directly calling _onComplete() here because we're still on HttpListener's async thread.
				// We need _onComplete() to be called on the main thread, so we store the auth token and set a flag
				// that will tell us when we should call _onComplete() in the Update() method, which always executes
				// on the main thread.
				_authToken = authToken;
				_shouldNotifyAuthTokenReceived = true;

				CheckForTokenRecieve();
			}
		}

		//Background Processes....
		static void CheckForTokenRecieve() {
			lock (_notifyAuthTokenLock) {
				// using a lock here because we'll be modifying _shouldNotifyAuthTokenReceived on both the main thread and on HttpListener's async thread.
				if (_shouldNotifyAuthTokenReceived) {
					if (_onComplete != null) {
						_onComplete(_authToken);
					}

					_shouldNotifyAuthTokenReceived = false;
				}
			}
		}

		/// <summary>
		/// Some HTML response content was passed in to the StartListening() method, and this is where we display it to the user.</summary>
		static void HandleListenerContextResponse(HttpListenerContext context) {
			byte[] buffer = Encoding.UTF8.GetBytes(_htmlResponseContent);
			context.Response.StatusCode = (int)HttpStatusCode.OK;
			context.Response.ContentType = "text/html";
			context.Response.ContentLength64 = buffer.Length;
			context.Response.OutputStream.Write(buffer, 0, buffer.Length);
			context.Response.OutputStream.Close();
		}
#else
		public static void BuildHttpListener() { }
#endif

		/// <summary>
		/// checks if time has expired far enough that a new auth token needs to be issued
		/// </summary>
		/// <returns></returns>
		public static IEnumerator CheckForRefreshOfToken() {
			if (DateTime.Now > SpreadsheetManager.Config.gdr.nextRefreshTime) {
				Debug.Log("Refreshing Token");

				var f = new Dictionary<string, string>();
				f.Add("client_id", SpreadsheetManager.Config.CLIENT_ID);
				f.Add("client_secret", SpreadsheetManager.Config.CLIENT_SECRET);
				f.Add("refresh_token", SpreadsheetManager.Config.gdr.refresh_token);
				f.Add("grant_type", "refresh_token");
				f.Add("scope", "");

				using (UnityWebRequest request = UnityWebRequest.Post("https://oauth2.googleapis.com/token", f)) {
					yield return request.SendWebRequest();

					GoogleDataResponse newGdr = JsonUtility.FromJson<GoogleDataResponse>(request.downloadHandler.text);
					SpreadsheetManager.Config.gdr.access_token = newGdr.access_token;
					SpreadsheetManager.Config.gdr.nextRefreshTime = DateTime.Now.AddSeconds(newGdr.expires_in);

#if UNITY_EDITOR
					EditorUtility.SetDirty(SpreadsheetManager.Config);
					AssetDatabase.SaveAssets();
#endif
				}
			}
		}
	}
}