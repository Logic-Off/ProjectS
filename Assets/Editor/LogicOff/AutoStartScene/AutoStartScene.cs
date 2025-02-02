using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LogicOff {
	[InitializeOnLoad]
	public class AutoStartScene {
		static AutoStartScene() => SetStartScene("Assets/Scenes/StartScene.unity");

		public static bool IsAutoSetStartScene {
			get => EditorPrefs.GetBool("EditorSetStartScene", true);
			set {
				EditorPrefs.SetBool("EditorSetStartScene", value);
				OnChangeStartScene(value);
			}
		}

		private static void OnChangeStartScene(bool value) {
			if (value)
				SetStartScene("Assets/Scenes/StartScene.unity");
			else
				EditorSceneManager.playModeStartScene = null;
		}

		private static void SetStartScene(string path) {
			if (!IsAutoSetStartScene)
				return;

			if (EditorSceneManager.playModeStartScene != null)
				return;

			var myWantedStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
			if (myWantedStartScene != null)
				EditorSceneManager.playModeStartScene = myWantedStartScene;
			else
				Debug.LogError("Could not find Scene " + path);
		}
	}
}