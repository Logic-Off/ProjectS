using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LogicOff.Toolbar {
	public class DropdownWithoutTarget<T> : VisualElement {
		public Button Button;

		private T _value;

		public T LastTarget {
			get => _value;
			set {
				_value = value;

				foreach (var listener in _listeners)
					listener.Invoke(_value);
			}
		}

		public List<T> Choices = new();

		private readonly List<Action<T>> _listeners = new(2);

		public void RegisterValueChangedCallback(Action<T> onChange) {
			if (_listeners.Contains(onChange))
				return;
			_listeners.Add(onChange);
		}

		public void UnregisterValueChangedCallback(Action<T> onChange) {
			if (!_listeners.Contains(onChange))
				return;
			_listeners.Remove(onChange);
		}

		public DropdownWithoutTarget(string style, string text, List<T> choices) {
			Button = LoadUxml($"Assets/Editor/LogicOff/Toolbar/{style}.uxml").Q<Button>();
			if (Button.enableRichText)
				Button.text = text;
			else
				Button.Q<Label>().text = text;

			Button.clicked += ShowMenu;
			Choices = choices;

			Add(Button);
		}

		private void ShowMenu() {
			GenericMenu menu = new GenericMenu();
			AddMenuItems(menu);
			menu.DropDown(Button.worldBound);
		}

		protected virtual void AddMenuItems(GenericMenu menu) {
			foreach (var choice in Choices)
				menu.AddItem(new GUIContent(choice.ToString()), false, () => ChangeValueFromMenu(choice));
		}

		private void ChangeValueFromMenu(T menuItem) => LastTarget = menuItem;

		public TemplateContainer LoadUxml(string name) {
			var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(name);
			var template = asset.Instantiate();
			return template;
		}
	}
}