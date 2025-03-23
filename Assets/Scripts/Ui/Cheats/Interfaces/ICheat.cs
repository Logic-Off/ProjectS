using System;
using System.Collections.Generic;

namespace Ui.Cheats {
	public interface ICheat {
		string Name { get; }
		int Order { get; }
		Dictionary<string, Action> ButtonCallbacks { get; }
		Dictionary<string, CheatElementData> Objects { get; }
		void Create();
		void Refresh();
	}
}