using System;
using System.Collections.Generic;

namespace Ui.Cheats {
	public abstract class ACheat : ICheat {
		public virtual string Name => this.GetType().Name;
		public abstract int Order { get; }

		private Dictionary<string, Action> _buttonCallbacks = new();
		public Dictionary<string, Action> ButtonCallbacks => _buttonCallbacks;
		private Dictionary<string, CheatElementData> _objects = new();
		public Dictionary<string, CheatElementData> Objects => _objects;

		public abstract void Create();
		public virtual void Refresh() { }

		protected void AddButton(string name, Action callback) {
			if (Objects.ContainsKey(name)) {
				D.Error("[ACheat]",$"Кнопка '{name}' не добавлена, дубликат");
				return;
			}
			var entry = new CheatElementData() {
				Type = ECheatElementType.Button,
				Name = name,
				Text = name
			};
			ButtonCallbacks[name] = callback;
			Objects.Add(name, entry);
		}

		protected void AddButton(string name, string text, Action callback) {
			if (Objects.ContainsKey(name)) {
				D.Error("[ACheat]",$"Кнопка '{name}' не добавлена, дубликат");
				return;
			}
			var entry = new CheatElementData() {
				Type = ECheatElementType.Button,
				Name = name,
				Text = text
			};
			ButtonCallbacks[name] = callback;
			Objects.Add(name, entry);
		}

		protected void AddHeader(string name) {
			if (Objects.ContainsKey(name)) {
				D.Error("[ACheat]",$"Заголовк '{name}' не добавлен, дубликат");
				return;
			}
			var entry = new CheatElementData() {
				Type = ECheatElementType.Header,
				Name = name,
				Text = name
			};
			Objects.Add(name, entry);
		}

		protected void AddText(string name, string text) {
			if (Objects.ContainsKey(name)) {
				D.Error("[ACheat]",$"Текст '{name}' не добавлен, дубликат");
				return;
			}
			var entry = new CheatElementData() {
				Type = ECheatElementType.Text,
				Name = name,
				Text = text
			};
			Objects.Add(name, entry);
		}
	}
}