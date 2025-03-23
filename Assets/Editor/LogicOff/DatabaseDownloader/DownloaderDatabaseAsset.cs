using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Ecs.Ability;
using Ecs.Character;
using Ecs.Item;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace LogicOff.DatabaseDownloader {
	[CreateAssetMenu(menuName = "Editor/DownloaderDatabaseAsset", fileName = "DownloaderDatabaseAsset")]
	public sealed class DownloaderDatabaseAsset : ScriptableObject {
		private DatabaseSheetsSettings _settings;
		private bool _isLoading;

		[FoldoutGroup("DatabasesFoldout"), SerializeField]
		private CharacterDatabaseAsset _characterDatabase;

		[FoldoutGroup("DatabasesFoldout"), SerializeField]
		private ItemsDatabaseAsset _itemsDatabase;

		[FoldoutGroup("DatabasesFoldout"), SerializeField]
		private AbilitiesDatabaseAsset _abilitiesDatabase;

		private void OnEnable() => _settings = Resources.Load<DatabaseSheetsSettings>("DownloadDatabasesSettings");

		[FoldoutGroup("Character", Expanded = true), Button("Characters")]
		public void DownloadCharacters() => OnDownload("Character", "Characters", _characterDatabase.Characters, new CharactersDownloader(), _characterDatabase);

		[FoldoutGroup("Character", Expanded = true), Button("Abilities")]
		public void DownloadAbilities() => OnDownload("Abilities", "Abilities", _abilitiesDatabase.Abilities, new AbilitiesDownloader(), _abilitiesDatabase);

		[FoldoutGroup("Items", Expanded = true), Button("Items")]
		public void DownloadItems() => OnDownload("Items", "Items", _itemsDatabase.All, new ItemsDownloader(), _itemsDatabase);

		[FoldoutGroup("Items", Expanded = true), Button("Weapons")]
		public void DownloadWeapons() => OnDownload("Items", "Weapons", _itemsDatabase.Weapons, new WeaponsDownloader(), _itemsDatabase);

		[FoldoutGroup("Items", Expanded = true), Button("ContainerSettings")]
		public void DownloadContainerSettings()
			=> OnDownload("Items", "ContainerSettings", _itemsDatabase.ContainerSettings, new ContainerSettingsDownloader(), _itemsDatabase);

		#region Downloader logic

		private void OnDownload<T, V>(string databaseName, string sheetName, List<T> list, V downloader, Object obj) where V : ADownloader<T> {
			var settings = _settings.Get(databaseName);
			var entry = settings.Sheets.Find(x => x.Name == sheetName);
			downloader.Set(settings.SpreadsheetId, entry, list);
			DownloadDatabase(downloader, () => Save(obj));
		}

		private void OnDownload<T, V>(string databaseName, string sheetName, Action<T[]> callaback, V downloader, Object obj) where V : ADownloader<T> {
			var settings = _settings.Get(databaseName);
			var entry = settings.Sheets.Find(x => x.Name == sheetName);
			downloader.Set(settings.SpreadsheetId, entry, callaback);
			DownloadDatabase(downloader, () => Save(obj));
		}

		private async Task DownloadDatabase(IDownloader downloader, Action callback = null) {
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

		private void Save(Object target) {
			EditorUtility.SetDirty(target);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		#endregion
	}
}