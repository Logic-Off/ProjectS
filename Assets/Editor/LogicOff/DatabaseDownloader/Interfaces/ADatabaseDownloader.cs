using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace LogicOff.DatabaseDownloader {
	public abstract class ADatabaseDownloader : Editor {
		protected DatabaseSheetsSettings _settings;
		protected List<IDownloader> _downloaders = new();
		private bool _foldout;
		protected bool _startLoading;
		protected bool _isLoading;

		protected virtual void OnEnable() {
			_settings = Resources.Load<DatabaseSheetsSettings>("DownloadDatabasesSettings");
		}

		protected virtual void OnDisable() {
			_downloaders.Clear();
		}

		protected async Task DownloadDatabase<T>(ADownloader<T> downloader, List<T> list, Action callback = null) {
			if (_isLoading)
				return;
			_isLoading = true;
			Debug.Log($"Start download: {downloader.Name}");
			var result = await downloader.DownloadSheet();
			Debug.Log($"End download: {downloader.Name}");
			list.Clear();
			list.AddRange(result);
			callback?.Invoke();
			_isLoading = false;
			await Task.Yield(); // На всякий пожарный 1 тик подождем
		}

		protected async Task DownloadDatabase(IDownloader downloader, Action callback = null) {
			if (_isLoading)
				return;
			_isLoading = true;
			var sw = new Stopwatch();
			sw.Start();
			Debug.Log($"Start download: {downloader.Name}");
			await downloader.Download();
			sw.Stop();
			Debug.Log($"End download: {downloader.Name}, downloading time: {(sw.ElapsedMilliseconds * 0.001f):F2}s");
			callback?.Invoke();
			await Task.Yield(); // На всякий пожарный 1 тик подождем
			_isLoading = false;
		}

		public override void OnInspectorGUI() {
			OnDrawButtons();

			if (GUILayout.Button("Download All"))
				OnDownloadAll();

			base.OnInspectorGUI();
		}

		protected virtual void OnDrawButtons() {
			if (GUILayout.Button("Download"))
				_foldout = !_foldout;

			if (!_foldout)
				return;

			foreach (var downloader in _downloaders)
				if (GUILayout.Button(downloader.Name))
					DownloadDatabase(downloader, Save);
		}

		protected virtual async Task OnDownloadAll() {
			if (_isLoading || _startLoading)
				return;

			_startLoading = true;
			var sw = new Stopwatch();
			sw.Start();
			foreach (var downloader in _downloaders)
				await DownloadDatabase(downloader);
			Save();
			sw.Stop();
			Debug.Log($"[ADatabaseDownloader] End download all, downloading time: {(sw.ElapsedMilliseconds * 0.001f):F2}s");
			_startLoading = false;
		}

		protected void Save() {
			EditorUtility.SetDirty(target);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
	}
}