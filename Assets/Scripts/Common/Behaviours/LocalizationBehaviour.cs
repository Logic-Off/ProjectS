using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Common {
	public class LocalizationBehaviour : LocalizeStringEvent {
		public void SetKey(LocalizationKey localizationKey) => SetKey(localizationKey.Key, localizationKey.Arguments);

		public void SetKey(string key, IList<object> arguments) {
			StringReference.Arguments = arguments;
			StringReference.TableReference = LocalizationSettings.AssetDatabase.DefaultTable;
			StringReference.TableEntryReference = key;

			RefreshString();
		}

		public void SetArguments(params object[] arguments) {
			StringReference.Arguments = arguments;
			RefreshString();
		}

		private void Awake() {
#if DEBUG
			if (OnUpdateString.GetPersistentEventCount() == 0)
				D.Error("[LocalizationBehaviour]", gameObject.name, " нет ивентов, перепроверить и назначить обновление текста");
#endif
		}

#if UNITY_EDITOR
		[HideInInspector] public string EditorKey;
		[HideInInspector] public TableReference EditorTable;
#endif
	}
}