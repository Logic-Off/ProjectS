using Ecs.Ui;
using TMPro;
using UnityEditor;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Common {
	[CustomEditor(typeof(LocalizationBehaviour), true)]
	public sealed class LocalizationBehaviourEditor : Editor {
		private LocalizationBehaviour _target;

		private void OnEnable() {
			_target = (target as LocalizationBehaviour)!;
			_target.StringReference.ValueChanged += OnChange;
		}

		private void OnDisable() {
			_target.StringReference.ValueChanged -= OnChange;
			_target = null;
		}

		private void OnChange(IVariable obj) {
			var tableReference = _target.StringReference.TableReference;
			var key = _target.StringReference.TableEntryReference.ResolveKeyName(UnityEditor.Localization.LocalizationEditorSettings.GetStringTableCollection(tableReference)?.SharedData);
			_target.EditorKey = key;
			_target.EditorTable = tableReference;
		}
	}

	public class LocalizationFastComponent {
		[UnityEditor.MenuItem("CONTEXT/TextMeshProUGUI/LocalizationBehaviour(Использовать вместо Localizae)")]
		static void LocalizeTMProText(UnityEditor.MenuCommand command) {
			var target = command.context as TextMeshProUGUI;
			SetupForLocalization(target);
		}

		[UnityEditor.MenuItem("CONTEXT/TextMeshProUGUI/LocalizationWidget")]
		static void LocalizeWidget(UnityEditor.MenuCommand command) {
			var target = command.context as TextMeshProUGUI;
			SetupForLocalization(target);
			var widget = UnityEditor.Undo.AddComponent(target.gameObject, typeof(LocalizationWidget)) as LocalizationWidget;
		}

		private static void SetupForLocalization(TextMeshProUGUI target) {
			var component = UnityEditor.Undo.AddComponent(target.gameObject, typeof(LocalizationBehaviour)) as LocalizationBehaviour;
			var setStringMethod = target.GetType().GetProperty("text").GetSetMethod();
			var methodDelegate =
				System.Delegate.CreateDelegate(typeof(UnityEngine.Events.UnityAction<string>), target, setStringMethod) as UnityEngine.Events.UnityAction<string>;
			UnityEditor.Events.UnityEventTools.AddPersistentListener(component.OnUpdateString, methodDelegate);
			component.OnUpdateString.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.EditorAndRuntime);
		}
	}
}