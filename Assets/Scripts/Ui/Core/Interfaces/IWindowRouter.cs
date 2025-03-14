using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ui {
	public interface IWindowRouter {
		void AddWindow(IWindow window);
		UniTask OpenWindow(EWindowName nextWindowName, EOpenWindowType openType = EOpenWindowType.Default, Action<IWindow> argument = null);
		void OpenWindow<TArguments>(EWindowName name, TArguments args, EOpenWindowType openType = EOpenWindowType.Default);
		void OnRoot();
		void OnBack();
		IWindow GetWindow(EWindowName name);
	}
}