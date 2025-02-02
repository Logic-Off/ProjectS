using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace LogicOff.Grabber {
	[CustomEditor(typeof(GrabberDatabase))]
	public sealed class GrabberEditor : Editor {
		private int _collectedObjects;
		private readonly List<(string, List<string>)> _objects = new();

		private GrabberDatabase _target;

		private void OnEnable() => _target = (GrabberDatabase)target;

		public override VisualElement CreateInspectorGUI() {
			var rootElement = new VisualElement();
			DefaultEditor.FillDefaultInspectorIMGUI(rootElement, serializedObject);

			var mainContainer = new VisualElement();
			rootElement.Add(mainContainer);

			mainContainer.Add(new Button(OnCollectNativeObjects) { text = "Collect native objects" });
			mainContainer.Add(new Button(OnSetAllRemoteObjectsTagPreload) { text = "On set preload tag" });
			mainContainer.Add(new Button(OnSetCustomTags) { text = "On set custom tags" });
			mainContainer.Add(new Button(OnSaveGroups) { text = "Save groups(После манипуляций с группами)" });
			mainContainer.Add(new Button(OnClearSceneObjects) { text = "Clear scene objects" });
			return rootElement;
		}

		private void OnSaveGroups() {
			foreach (var assetGroup in AddressableAssetSettingsDefaultObject.Settings.groups)
				EditorUtility.SetDirty(assetGroup);
			AssetDatabase.SaveAssets();
		}

		private void OnSetAllRemoteObjectsTagPreload() {
			D.Log("[GrabberEditor]", "Start set preload tags");
			var settings = AddressableAssetSettingsDefaultObject.Settings;
			foreach (var assetGroup in settings.groups) {
				var schema = assetGroup.GetSchema(typeof(BundledAssetGroupSchema)) as BundledAssetGroupSchema;
				if (schema == null)
					continue;
				var buildKey = schema.BuildPath.GetName(settings).Split('.')[0];
				var isRemote = buildKey.Equals("Remote");
				foreach (var entry in assetGroup.entries) {
					var hasLabel = entry.labels.Contains("Preload");
					if (!hasLabel && isRemote)
						entry.labels.Add("Preload");
					else if (hasLabel && !isRemote)
						entry.labels.Remove("Preload");
				}

				settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryAdded, assetGroup, false, true);
			}

			AssetDatabase.Refresh();
			D.Log("[GrabberEditor]", "Ent set preload tags");
		}

		private void OnSetCustomTags() {
			D.Log("[GrabberEditor]", "Start set customTags");
			var settings = AddressableAssetSettingsDefaultObject.Settings;
			foreach (var assetGroup in settings.groups) {
				foreach (var tagSettings in _target.GroupTags) {
					if (tagSettings.Groups.Contains(assetGroup.name)) {
						OnAddCustomTags(assetGroup, tagSettings);
						continue;
					}

					OnRemoveCustomTags(assetGroup, tagSettings);
				}

				settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryAdded, assetGroup, false, true);
			}

			AssetDatabase.Refresh();
			D.Log("[GrabberEditor]", "Ent set customTags");
		}

		private static void OnAddCustomTags(AddressableAssetGroup assetGroup, GrabberDatabase.TagSettings tagSettings) {
			foreach (var entry in assetGroup.entries)
			foreach (var tag in tagSettings.Tags)
				if (!entry.labels.Contains(tag))
					entry.labels.Add(tag);
		}

		private static void OnRemoveCustomTags(AddressableAssetGroup assetGroup, GrabberDatabase.TagSettings tagSettings) {
			foreach (var entry in assetGroup.entries)
			foreach (var tag in tagSettings.Tags)
				if (entry.labels.Contains(tag))
					entry.labels.Remove(tag);
		}

		private async void OnCollectNativeObjects() {
			_collectedObjects = 0;
			D.Log("[GrabberEditor]", "Start collect native objects");
			var startTime = Time.realtimeSinceStartup;
			await Task.Yield();
			await OnCollectInstallers(_target.RemoteInstallers, "RemoteInstallers", false);
			await Task.Yield();
			OnCollectReferencesInstallers(_target.RemoteReferencesInstallers, "RemoteReferencesInstallers", false);
			foreach (var sceneSettings in _target.Scenes) {
				await Task.Yield();
				CollectAll(sceneSettings.Scene, sceneSettings.Group, sceneSettings.IsDirtyCollect);

				D.Log("[GrabberEditor]", "Collect scene: ", sceneSettings.Scene.name);
			}

			await Task.Yield();
			await OnCollectInstallers(_target.LocalInstallers, "LocalInstallers", false);
			await Task.Yield();
			await OnCollectReferencesInstallers(_target.LocalReferencesInstallers, "LocalReferencesInstallers", false);

			var settings = AddressableAssetSettingsDefaultObject.Settings;

			await Task.Yield();
			RemoveLocaleFilesFromRemote();
			D.Log("[GrabberEditor]", "Remove local files from remote");

			D.Log("[GrabberEditor]", "All objects collected:  ", _collectedObjects, " time: ", Time.realtimeSinceStartup - startTime);

			foreach (var assetGroup in settings.groups) {
				foreach (var entry in assetGroup.entries)
					settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, false);
				settings.SetDirty(AddressableAssetSettings.ModificationEvent.GroupMoved, assetGroup, false);
			}

			if (settings.IsPersisted)
				EditorUtility.SetDirty(settings);

			AssetDatabase.Refresh();
			_objects.Clear();
		}

		private async void OnClearSceneObjects() {
			D.Log("[GrabberEditor]", "Start clear scene objects");
			await Task.Yield();
			var startTime = Time.realtimeSinceStartup;
			foreach (var sceneSettings in _target.Scenes)
				OnClearPackGroups(sceneSettings.Group);
			OnClearPackGroups("LocalBuildings");
			OnClearPackGroups("LocalInstallers");
			OnClearPackGroups("RemoteInstallers");
			OnClearPackGroups("LocalReferencesInstallers");
			OnClearPackGroups("RemoteReferencesInstallers");

			OnClear("LocalDuplicates");
			OnClear("RemoteDuplicates");

			var settings = AddressableAssetSettingsDefaultObject.Settings;
			SetDirtyGroup(settings, "LocalDuplicates");
			SetDirtyGroup(settings, "RemoteDuplicates");
			foreach (var assetGroup in settings.groups)
				settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryRemoved, assetGroup, false, true);
			AssetDatabase.Refresh();
			D.Log("[GrabberEditor]", "Clear time: ", Time.realtimeSinceStartup - startTime);
		}

		private void OnClearPackGroups(string groupName) {
			OnClear($"{groupName}Prefabs");
			OnClear($"{groupName}Meshes");
			OnClear($"{groupName}Materials");
			OnClear($"{groupName}Textures");
		}

		private void SetDirtyGroup(AddressableAssetSettings settings, string groupName) {
			var group = settings.FindGroup(groupName);
			settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryRemoved, group, true, true);
		}

		private void CollectAll(SceneAsset obj, string groupName, bool isDirtyCollect) {
			var asset = EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(obj), OpenSceneMode.Additive);
			foreach (var rootGameObject in asset.GetRootGameObjects())
				CollectAll(rootGameObject, groupName, isDirtyCollect);

			EditorSceneManager.CloseScene(asset, true);
		}

		private void CollectAll(GameObject obj, string groupName, bool isDirtyCollect) {
			CreatePrefabs(obj, $"{groupName}Prefabs", isDirtyCollect);
			CreateMeshes(obj, $"{groupName}Meshes", isDirtyCollect);
			CreateMaterials(obj, groupName, isDirtyCollect);

			_target.Transforms.Clear();
			_target.Meshes.Clear();
			_target.Materials.Clear();
			_target.Textures.Clear();
			_target.Sprites.Clear();
		}

		private async Task OnCollectInstallers(
			List<ScriptableObject> installers,
			string groupName,
			bool isDirtyCollect
		) {
			foreach (var installer in installers)
			foreach (var field in installer.GetType().GetRuntimeFields()) {
				var value = field.GetValue(installer);
				if (value is MonoBehaviour monoBehaviour)
					CollectAll(monoBehaviour.gameObject, groupName, isDirtyCollect);
				else if (value is AssetReference reference)
					CollectAll(reference.editorAsset as GameObject, groupName, isDirtyCollect);
			}
		}

		private async Task OnCollectReferencesInstallers(
			List<ScriptableObject> installers,
			string groupName,
			bool isDirtyCollect
		) {
			D.Error("[GrabberEditor]","Тут скрыта ошибка, надо раскоментить");
			// foreach (var installer in installers)
			// foreach (var field in installer.GetType().GetRuntimeFields()) {
			// 	var value = field.GetValue(installer);
			// 	if (value is IEnumerable<PrefabRecord> array)
			// 		foreach (var prefabRecord in array) {
			// 			if (prefabRecord.Prefab.editorAsset == null)
			// 				continue;
			// 			CollectAll(prefabRecord.Prefab.editorAsset as GameObject, groupName, isDirtyCollect);
			// 		}
			// 	else if (value is AssetReference reference)
			// 		CollectAll(reference.editorAsset as GameObject, groupName, isDirtyCollect);
			// }
		}

		private void CreatePrefabs(GameObject target, string groupName, bool isDirtyCollect) {
			if (target == null)
				return;
			var list = new List<string>();

			CollectPrefab(target.transform, list);
			CollectChilds(target.transform, list);

			_objects.Add((groupName, list));
			var settings = AddressableAssetSettingsDefaultObject.Settings;
			OnCreate(groupName, isDirtyCollect, settings, list);
		}

		private static void CollectChilds(Transform target, List<string> list) {
			for (var i = 0; i < target.transform.childCount; i++) {
				var child = target.transform.GetChild(i);
				CollectPrefab(child, list);
				if (child.childCount > 0)
					CollectChilds(child, list);
			}
		}

		private static void CollectPrefab(Transform transform, List<string> list) {
			var prefab = PrefabUtility.GetPrefabAssetType(transform);
			if (prefab != PrefabAssetType.Regular && prefab != PrefabAssetType.Model && prefab != PrefabAssetType.Variant)
				return;
			var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(transform);
			if (!path.Contains("Assets"))
				return;
			var guid = AssetDatabase.GUIDFromAssetPath(path).ToString();
			if (!list.Contains(guid))
				list.Add(guid);
		}

		private void CreateMaterials(GameObject target, string groupName, bool isDirtyCollect) {
			if (target == null)
				return;

			var list = new List<string>();
			var textures = new List<string>();
			foreach (var meshRenderer in target.GetComponentsInChildren<MeshRenderer>(true))
				CollectMaterial(meshRenderer);
			foreach (var meshRenderer in target.GetComponentsInChildren<SkinnedMeshRenderer>(true))
				CollectMaterial(meshRenderer);

			void CollectMaterial(Renderer meshRenderer) {
				foreach (var material in meshRenderer.sharedMaterials) {
					if (material == null)
						continue;

					var pathMaterial = AssetDatabase.GetAssetPath(material);
					if (!pathMaterial.Contains("Assets"))
						continue;
					var guid = AssetDatabase.GUIDFromAssetPath(pathMaterial).ToString();
					if (list.Contains(guid))
						continue;

					list.Add(guid);

					foreach (var texturePropertyName in material.GetTexturePropertyNames()) {
						var texture = material.GetTexture(texturePropertyName);
						if (texture == null)
							continue;
						var path = AssetDatabase.GetAssetPath(texture);
						if (!path.Contains("Assets"))
							continue;
						var textureGuid = AssetDatabase.GUIDFromAssetPath(path).ToString();
						if (textures.Contains(textureGuid))
							continue;

						textures.Add(textureGuid);
					}
				}
			}

			foreach (var image in target.GetComponentsInChildren<Image>(true)) {
				var sprite = image.sprite;
				if (sprite == null)
					continue;
				var path = AssetDatabase.GetAssetPath(sprite);
				if (!path.Contains("Assets"))
					continue;
				var guid = AssetDatabase.GUIDFromAssetPath(path).ToString();
				if (textures.Contains(guid))
					continue;
				if (CheckSpriteInAtlases(sprite))
					continue;

				textures.Add(guid);
			}

			_objects.Add(($"{groupName}Materials", list));
			_objects.Add(($"{groupName}Textures", textures));
			var settings = AddressableAssetSettingsDefaultObject.Settings;
			OnCreate($"{groupName}Materials", isDirtyCollect, settings, list);
			OnCreate($"{groupName}Textures", isDirtyCollect, settings, textures);
		}

		private bool CheckSpriteInAtlases(Sprite sprite) {
			foreach (var atlas in _target.Atlases)
				if (atlas.CanBindTo(sprite)) {
					D.Error("[GrabberEditor]", $"Спрайт '{sprite.name}' в атласе");
					return true;
				}

			return false;
		}

		private void CreateMeshes(GameObject target, string groupName, bool isDirtyCollect) {
			if (target == null)
				return;

			var list = new List<string>();

			foreach (var meshFilter in target.GetComponentsInChildren<MeshFilter>(true)) {
				if (meshFilter.sharedMesh == null || meshFilter.sharedMesh.name == "Plane Instance")
					continue;

				AddMesh(meshFilter.sharedMesh);
			}

			foreach (var meshFilter in target.GetComponentsInChildren<SkinnedMeshRenderer>(true)) {
				if (meshFilter.sharedMesh == null || meshFilter.sharedMesh.name == "Plane Instance")
					continue;

				AddMesh(meshFilter.sharedMesh);
			}

			void AddMesh(Mesh mesh) {
				var guid = AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(mesh)).ToString();
				if (!list.Contains(guid))
					list.Add(guid);
			}

			_objects.Add((groupName, list));
			var settings = AddressableAssetSettingsDefaultObject.Settings;
			OnCreate(groupName, isDirtyCollect, settings, list);
		}

		private void OnCreate(
			string groupName,
			bool isDirtyCollect,
			AddressableAssetSettings settings,
			List<string> list
		) {
			var group = settings.groups.Find(x => x.Name == groupName);
			if (group == null)
				group = settings.CreateGroup(groupName, false, false, false, new List<AddressableAssetGroupSchema>());

			CreateAndAddToGroup(list, settings, group, isDirtyCollect);
		}

		private void OnClear(string groupName) {
			var settings = AddressableAssetSettingsDefaultObject.Settings;
			var group = settings.groups.Find(x => x.Name == groupName);
			if (group == null)
				return;
			var entries = new List<AddressableAssetEntry>(group.entries);
			foreach (var entry in entries)
				settings.RemoveAssetEntry(entry.guid, false);
		}

		private void CreateAndAddToGroup(
			IEnumerable<string> values,
			AddressableAssetSettings settings,
			AddressableAssetGroup group,
			bool isDirty
		) {
			foreach (var guid in values) {
				var path = AssetDatabase.GUIDToAssetPath(guid);
				if (path.Contains("Library/") ||
				    path.Contains("Generated Materials")) // Generated Materials хз как отсечь кроме как саму папку, гуид есть, а объект как таковой не считается сгенереным
					continue;

				var entry = settings.FindAssetEntry(guid);
				if (entry != null)
					continue;
				// Перемещать запись нам не надо, но создать необходимо(там дублируется код на поиск), метод по созданию приватный, используем этот
				// Если есть необходимость создать напрямую, то юзаем рефлексию или потрошим ассет
				// Учитывая что на данный момент дикая связь пакетов инициализированных, то только рефлексия
				settings.CreateOrMoveEntry(guid, group, false, false);
			}

			_collectedObjects += values.Count();
		}

		private void RemoveLocaleFilesFromRemote() {
			var settings = AddressableAssetSettingsDefaultObject.Settings;
			var localGroups = new List<AddressableAssetGroup>();
			var remoteGroups = new List<AddressableAssetGroup>();
			foreach (var assetGroup in settings.groups) {
				var schema = assetGroup.GetSchema(typeof(BundledAssetGroupSchema)) as BundledAssetGroupSchema;
				if (schema != null) {
					var buildKey = schema.BuildPath.GetName(settings).Split('.')[0];
					var loadKey = schema.LoadPath.GetName(settings).Split('.')[0];
					if (buildKey == "Local" && loadKey == "Local")
						localGroups.Add(assetGroup);
					else if (buildKey == "Remote" && loadKey == "Remote")
						remoteGroups.Add(assetGroup);
					else
						D.Log("[GrabberEditor]", schema.BuildPath.GetValue(settings), schema.LoadPath.GetValue(settings));
				} else {
					D.Log("[GrabberEditor]", assetGroup.Name, "Schema is null");
				}
			}

			foreach (var assetGroup in localGroups)
			foreach (var entry in assetGroup.entries)
				RemoveRemoteEntry(entry);

			void RemoveRemoteEntry(AddressableAssetEntry entry) {
				foreach (var assetGroup in remoteGroups) {
					var list = assetGroup.entries.ToList();
					var remove = new List<AddressableAssetEntry>();
					for (var i = 0; i < list.Count; i++) {
						if (list[i].guid != entry.guid)
							continue;
						remove.Add(list[i]);
					}

					foreach (var assetEntry in remove) {
						assetGroup.RemoveAssetEntry(assetEntry, false);
						_collectedObjects--;
					}
				}
			}
		}
	}
}