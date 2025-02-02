using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace LogicOff.Toolbar {
	public sealed class ToolbarBuilder {
		private ToolbarView _view;

		public ToolbarBuilder(VisualElement parent) {
			_view = new ToolbarView(parent);

			CreateElements(_view);
		}

		private void CreateElements(ToolbarView view) {
			ToolbarPreferences.instance.OnCollectScenes();
			foreach (var entry in ToolbarPreferences.instance.Entries) {
				if (!entry.IsVisible)
					continue;
				if (entry.Assembly == string.Empty) {
					Debug.LogError($"{entry.Name}: need to add a Assembly");
					continue;
				}

				var assembly = Assembly.Load(entry.Assembly);

				if (entry.ClassFullName == string.Empty) {
					Debug.LogError($"{entry.Name}: need to add a ClassName(Type.FullName)");
					continue;
				}

				var type = assembly.GetType(entry.ClassFullName);

				if (entry.ClassFullName == string.Empty) {
					Debug.LogError($"{entry.Name}: need to add a Method");
					continue;
				}

				var method = type.GetMethod(entry.Method);
				OnCreateElement(view, entry, method);
			}
		}

		private static void OnCreateElement(ToolbarView view, ToolbarSettingsEntry entry, MethodInfo method) {
			switch (entry.Type) {
				case EToolbarObjectType.Button:
					view.CreateButton(entry, method);
					break;
				case EToolbarObjectType.Toggle:
					view.CreateToggle(entry, method);
					break;
				case EToolbarObjectType.Dropdown:
					view.CreateToolsDropdown(entry, method);
					break;
			}
		}

		public void Redraw() {
			_view.Reset();

			CreateElements(_view);
		}
	}
}