using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Ui {
	/// <summary>
	///
	/// </summary>
	[AddComponentMenu("Localization/Localization(Main)")]
	public class LocalizationBehaviour : LocalizeStringEvent {
		private void Awake() {
#if DEBUG
			if (OnUpdateString.GetPersistentEventCount() == 0)
				D.Error("[LocalizationBehaviour]", gameObject.name, " нет ивентов, перепроверить и назначить обновление текста");
#endif
		}

		public void SetKey(LocalizationKey value) => SetKey(value.Key, value.Arguments);

		public void SetKey(string key, IList<object> arguments) {
			StringReference.Arguments = arguments;
			StringReference.TableReference = LocalizationSettings.AssetDatabase.DefaultTable;
			StringReference.TableEntryReference = key;

			RefreshString();
		}

#if UNITY_EDITOR
		// Кэш значений
		[HideInInspector] public string EditorKey;
		[HideInInspector] public TableReference EditorTable;
#endif
	}
}