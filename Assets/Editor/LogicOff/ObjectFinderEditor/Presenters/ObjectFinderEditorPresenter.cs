using System;
using Common;
using LogicOff;
using ObjectFinderEditor.Scripts.Databases;
using UnityEditor;

namespace ObjectFinderEditor.Scripts {
	public sealed class ObjectFinderEditorPresenter : IDisposable {
		public readonly IEventProperty<ObjectFinderScriptableObject> Database =
			new EventProperty<ObjectFinderScriptableObject>();

		public readonly IEventProperty<bool> Sort = new EventProperty<bool>();
		public readonly IEventProperty<bool> WithAssetGroups = new EventProperty<bool>();
		public readonly IEventProperty<bool> DependenciesMode = new EventProperty<bool>();
		public readonly ISignal OnFind = new Signal();
		public readonly ISignal OnClear = new Signal();
		public readonly ISignal OnRefreshPaths = new Signal();

		public void Dispose() {
			Sort?.Dispose();
			WithAssetGroups?.Dispose();
			DependenciesMode?.Dispose();
			Database?.Dispose();
			OnFind?.Dispose();
			OnClear?.Dispose();
			OnRefreshPaths?.Dispose();
		}

		public void OnData(ObjectFinderScriptableObject target) {
			Database.Value = target;
			WithAssetGroups.Value = EditorPrefs.GetBool("ObjectFinder.WithAssetGroupsToggle.Value", false);
			DependenciesMode.Value = EditorPrefs.GetBool("ObjectFinder.DependenciesModeToggle.Value", false);
		}
	}
}