using System;
using System.Collections.Generic;
using LogicOff.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

namespace LogicOff.Grabber {
	[CreateAssetMenu(menuName = "Editor/Databases/Grabber", fileName = "GrabberDatabase")]
	public sealed class GrabberDatabase : ScriptableObject {
#if UNITY_EDITOR
		[HideInInspector] public List<Mesh> Meshes = new();
		[HideInInspector] public List<Material> Materials = new();
		[HideInInspector] public List<Texture> Textures = new();
		[HideInInspector] public List<Sprite> Sprites = new();
		[HideInInspector] public List<Transform> Transforms = new();
		public List<ScriptableObject> LocalInstallers = new();
		public List<ScriptableObject> RemoteInstallers = new();
		public List<ScriptableObject> LocalReferencesInstallers = new();
		public List<ScriptableObject> RemoteReferencesInstallers = new();
		public List<SpriteAtlas> Atlases = new();

		public List<SceneSettings> Scenes;

		public List<TagSettings> GroupTags = new();

		[Serializable]
		public struct TagSettings {
			public List<string> Tags;
			public List<string> Groups;
		}

		[Serializable]
		public struct SceneSettings {
			public string Group;
			public SceneAsset Scene;
			public bool IsDirtyCollect;
		}
#endif
	}
}