using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LogicOff.Toolbar {
	public sealed class ToolbarView {
		private List<VisualElement> _elements = new();
		private VisualElement _leftContainer;
		private VisualElement _rightContainer;

		public ToolbarView(VisualElement parent) {
			_leftContainer = parent.Q<VisualElement>("ToolbarZoneLeftAlign");
			_rightContainer = parent.Q<VisualElement>("ToolbarZoneRightAlign");
		}

		public void CreateButton(ToolbarSettingsEntry entry, MethodInfo method) {
			var button = LoadUxml($"Assets/Editor/LogicOff/Toolbar/{entry.Style}.uxml").Q<Button>();
			if (button.enableRichText)
				button.text = entry.Name;
			else
				button.Q<Label>().text = entry.Name;
			button.clicked += () => method.Invoke(null, null);
			AddToContainer(button, entry.ZoneAlign);
			_elements.Add(button);
		}

		public void CreateToggle(ToolbarSettingsEntry entry, MethodInfo method) {
			var button = LoadUxml($"Assets/Editor/LogicOff/Toolbar/{entry.Style}.uxml").Q<Button>();
			button.style.backgroundColor = GetToggleColor(entry.Options[0]);
			if (button.enableRichText)
				button.text = entry.Name;
			else
				button.Q<Label>().text = entry.Name;
			button.clicked += () => {
				method.Invoke(null, null);
				button.style.backgroundColor = GetToggleColor(entry.Options[0]);
			};
			AddToContainer(button, entry.ZoneAlign);
			_elements.Add(button);
		}

		private static StyleColor GetToggleColor(string flagKey) {
			return new StyleColor(
				EditorPrefs.GetBool(flagKey)
					? new Color(0f, 0.5f, 0.1f)
					: new Color(0.235f, 0.235f, 0.235f)
			);
		}

		public void CreateToolsDropdown(ToolbarSettingsEntry entry, MethodInfo method) {
			var dropdown = new DropdownWithoutTarget<string>(entry.Style, entry.Name, entry.Options);
			dropdown.RegisterValueChangedCallback(x => method.Invoke(null, new[] { x }));
			var hovered = AddHoveredObject(dropdown);
			hovered.Add(dropdown.Button);
			AddToContainer(dropdown, entry.ZoneAlign);
			_elements.Add(dropdown);
		}

		private void AddToContainer(VisualElement element, EToolbarZoneAlign align) {
			if (align == EToolbarZoneAlign.Left)
				_leftContainer.Add(element);
			else
				_rightContainer.Add(element);
		}

		public static VisualElement AddHoveredObject(VisualElement element) {
			var hovered = new VisualElement();
			hovered.name = element.name + "_hovered";
			element.Add(hovered);
			return hovered;
		}

		public void Reset() {
			foreach (var element in _elements)
				element.RemoveFromHierarchy();
			_elements.Clear();
		}

		public TemplateContainer LoadUxml(string name) {
			var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(name);
			var template = asset.Instantiate();
			return template;
		}
	}
}