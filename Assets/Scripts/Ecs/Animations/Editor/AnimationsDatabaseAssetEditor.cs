using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Ecs.Animations {
	[CustomEditor(typeof(AnimationsDatabaseAsset))]
	public sealed class AnimationsDatabaseAssetEditor : Editor {
		private AnimationsDatabaseAsset _target;

		void OnEnable() => _target = serializedObject.targetObject as AnimationsDatabaseAsset;

		public override void OnInspectorGUI() {
			if (GUILayout.Button("Update settings"))
				OnUpdateSettings();

			base.OnInspectorGUI();
		}

		private void OnUpdateSettings() {
			for (var i = 0; i < _target.All.Count; i++) {
				var entry = _target.All[i];
				var clipField = entry.GetType().GetRuntimeFields().First(x => x.Name == "_clip");
				var clip = (AnimationClip) clipField.GetValue(entry);
				if (clip == null)
					continue;

				var stopTimeInfo = clip.GetType().GetRuntimeProperties().First(x => x.Name == "stopTime");
				var value = (float) stopTimeInfo.GetValue(clip);
				entry.Time = value;
				_target.All[i] = entry;
			}

			serializedObject.Update();
			serializedObject.ApplyModifiedProperties();
		}
	}
}