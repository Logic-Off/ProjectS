using System;
using Cysharp.Threading.Tasks;

namespace Ui {
	public interface IMainWindowController {
		void SetMainWindow(EWindowName name);
		UniTask OpenWindow(EWindowName nextWindowName, EOpenWindowType openType = EOpenWindowType.Default, Action<IWindow> argument = null);
	}
}