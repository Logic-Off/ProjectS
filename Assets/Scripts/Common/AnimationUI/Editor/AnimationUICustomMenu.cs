using UnityEditor;
using UnityEngine;

namespace Common {
	public class AnimationUICustomMenu {
		[MenuItem("GameObject/UI/Create AnimationUI")]
		static void CreateAnimationUI(MenuCommand menuCommand) {
			var selected = Selection.activeGameObject;
			var createdGo = new GameObject("AnimationUI");
			createdGo.AddComponent<AnimationUI>();
			GameObjectUtility.SetParentAndAlign(createdGo, selected);
			Undo.RegisterCreatedObjectUndo(createdGo, "Created +" + createdGo.name);
		}
	}
}