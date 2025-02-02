using System.Collections.Generic;
using System.IO;
using ObjectFinderEditor.Scripts.Databases;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ObjectFinderEditor.Scripts {
	public sealed class ObjectFinderEditorModel {
		private List<string> _allPaths;
		private List<string> types = new() { "prefab", "asset", "unity", "mat", "anim", "spriteatlas" };
		private Dictionary<string, List<string>> _objects = new();

		private readonly ObjectFinderEditorPresenter _presenter;

		public List<string> AllPaths {
			get {
				if (_allPaths != null)
					return _allPaths;

				OnRefreshPaths();
				return _allPaths;
			}
		}

		public ObjectFinderEditorModel(ObjectFinderEditorPresenter presenter) => _presenter = presenter;

		public void OnFindObjects() {
			if (_presenter.Database.Value == null)
				return;

			var database = _presenter.Database.Value;
			var targets = database.Targets;
			var foundObjects = database.FoundObjects;
			foundObjects.Clear();
			var time = Time.realtimeSinceStartup;
			var dictionary = FindObjects(targets);
			foreach (var target in targets)
				if (dictionary.ContainsKey(target))
					foundObjects.Add(new ObjectFinderFoundObjectEntry(target, dictionary[target]));

			if (_presenter.Sort.Value)
				foundObjects.Sort((a, b) => a.Values.Count.CompareTo(b.Values.Count));

			D.Log("[ObjectFinderEditorModel.FindObjectsTime]", Time.realtimeSinceStartup - time);
		}

		private Dictionary<Object, List<Object>> FindObjects(List<Object> targets) {
			var objects = new Dictionary<Object, List<Object>>();

			if (_presenter.DependenciesMode.Value)
				FindFromDependenciesWithCache(targets, objects);
			else
				FindInEachFile(targets, objects);

			return objects;
		}

		private void FindInEachFile(List<Object> targets, Dictionary<Object, List<Object>> objects) {
			foreach (var path in AllPaths) {
				var text = File.ReadAllText(path);
				foreach (var target in targets) {
					var targetPath = AssetDatabase.GetAssetPath(target);
					var guid = AssetDatabase.GUIDFromAssetPath(targetPath).ToString();
					if (!text.Contains(guid))
						continue;
					if (!objects.ContainsKey(target))
						objects.Add(target, new List<Object>());

					var assetPath = path.Replace(Application.dataPath, "Assets");
					var otherObject = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
					if (!_presenter.WithAssetGroups.Value && otherObject is AddressableAssetGroup ||
					    otherObject is ObjectFinderScriptableObject) // Не оптимально, но пофигу.
						continue;
					objects[target].Add(otherObject);
				}
			}
		}

		private void FindFromDependenciesWithCache(List<Object> targets, Dictionary<Object, List<Object>> objects) {
			if (_objects.Count == 0) {
				foreach (var path in AllPaths) {
					var dependencies = AssetDatabase.GetDependencies(path, false);
					_objects.Add(path, new List<string>(dependencies));
				}
			}

			foreach (var target in targets)
				objects.Add(target, new List<Object>());

			foreach (var (path, dependencies) in _objects) {
				foreach (var target in targets) {
					var targetPath = AssetDatabase.GetAssetPath(target);
					if (!dependencies.Contains(targetPath))
						continue;
					var otherObject = AssetDatabase.LoadAssetAtPath<Object>(path);
					if (!_presenter.WithAssetGroups.Value && otherObject is AddressableAssetGroup ||
					    otherObject is ObjectFinderScriptableObject)
						continue;
					objects[target].Add(otherObject);
				}
			}
		}

		private static string[] GetPathAllObjects(string type) => Directory.GetFiles(
			Application.dataPath,
			$"*.{type}",
			SearchOption.AllDirectories
		);

		public void OnClear() {
			var database = _presenter.Database.Value;
			database.Targets.Clear();
			database.FoundObjects.Clear();
		}

		public void OnSort(bool value) {
			if (_presenter.Database.Value == null)
				return;
			var database = _presenter.Database.Value;
			var targets = database.Targets;
			var foundObjects = database.FoundObjects;

			if (_presenter.Sort.Value)
				foundObjects.Sort((a, b) => a.Values.Count.CompareTo(b.Values.Count));
			else {
				for (var i = 0; i < targets.Count - 1; i++) {
					for (var j = i; j < foundObjects.Count; j++) {
						if (foundObjects[j].Target != targets[i])
							continue;
						var temp = foundObjects[i];
						foundObjects[i] = foundObjects[j];
						foundObjects[j] = temp;
						break;
					}
				}
			}
		}

		public void OnRefreshPaths() {
			if (_allPaths == null)
				_allPaths = new List<string>();
			else
				_allPaths.Clear();

			foreach (var type in types)
			foreach (var path in GetPathAllObjects(type))
				_allPaths.Add(path.Replace(Application.dataPath, "Assets"));
		}

		public void OnWithAssetGroups(bool value) => EditorPrefs.SetBool("ObjectFinder.WithAssetGroupsToggle.Value", value);
		public void OnDependenciesMode(bool value) => EditorPrefs.SetBool("ObjectFinder.DependenciesModeToggle.Value", value);
	}
}