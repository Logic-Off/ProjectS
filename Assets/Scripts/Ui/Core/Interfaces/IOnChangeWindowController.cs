using System;

namespace Ui {
	public interface IOnChangeWindowController {
		void SubscribeWindowChange(Action<IWindow> callback);
	}
}