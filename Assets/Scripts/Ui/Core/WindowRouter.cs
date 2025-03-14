using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utopia;

namespace Ui {
	[InstallerGenerator(InstallerId.Ui, 90)]
	public class WindowRouter : IDisposable, IWindowRouter, IMainWindowController, IOnChangeWindowController {
		private readonly Dictionary<EWindowName, IWindow> _activatedWindows = new();
		private readonly Stack<IWindow> _stack = new();
		private readonly Dictionary<EWindowName, IWindow> _windows = new();
		private IWindow _mainWindow;
		private Action<IWindow> _onWindowChange = window => { };
		private readonly Queue<(EWindowName, Action<IWindow>)> _queue = new();

		public void Dispose() {
			foreach (var window in _activatedWindows.Values)
				window.Dispose();
			_queue.Clear();
		}

		public void AddWindow(IWindow window) {
			if (_windows.ContainsKey(window.Name))
				throw new Exception($"Window with name {window.Name} was added already!");
			_windows[window.Name] = window;
		}

		public void SetMainWindow(EWindowName name) => _mainWindow = GetWindow(name);

		public async UniTask OpenWindow(
			EWindowName nextWindowName,
			EOpenWindowType openType = EOpenWindowType.Default,
			Action<IWindow> argument = null
		) {
			var nextWindow = GetWindow(nextWindowName);
#if DEBUG
			D.Warning("[WindowsController.OpenWindow]", nextWindow.Name);
#endif
			var currentWindow = _stack.Count > 0 ? _stack.Peek() : null;

			if (openType is EOpenWindowType.Queue && currentWindow != null && currentWindow.Name != _mainWindow.Name) {
				_queue.Enqueue((nextWindowName, argument));
				return;
			}

			if (nextWindow == currentWindow) {
				argument?.Invoke(currentWindow);
				return;
			}

			if (_stack.Count > 0)
				HandleOpenWindow(openType, currentWindow);

			OnCloseCurrentPopup(currentWindow, nextWindow);

			_stack.Push(nextWindow);

			argument?.Invoke(nextWindow);
			OnChangeWindow(nextWindow);

			// Ожидаем загрузку окна, попапы могут лезть вместе с первым открытием панели, после первого открытия условия не пройдут
			if (nextWindow is IPopUp && currentWindow != null && !(currentWindow is IPopUp))
				while (currentWindow.InProcessLoaded)
					await UniTask.Yield();

			nextWindow.OnOpen();
			nextWindow.OnShow();
		}

		public void OpenWindow<TArguments>(EWindowName name, TArguments args, EOpenWindowType openType = EOpenWindowType.Default)
			=> OpenWindow(name, openType, x => OnOpen(x, args));

		private void OnOpen<T>(IWindow window, T argument) {
			try {
				((IOpenWindow<T>) window).Open(argument);
			} catch (Exception e) {
				D.Error("[WindowsController]", window.Name, ", argument type: ", argument.GetType().Name);
				Debug.LogException(e);
				throw;
			}
		}

		private void HandleOpenWindow(EOpenWindowType openType, IWindow currentWindow) {
			switch (openType) {
				case EOpenWindowType.RemoveCurrent:
					_stack.Pop();
					break;
				case EOpenWindowType.ClearStack:
					if (currentWindow is IPopUp)
						GetFirstWindow().OnHide();

					_stack.Clear();
					break;
				case EOpenWindowType.Default:
				case EOpenWindowType.Queue:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(openType), openType, null);
			}
		}

		private void OnCloseCurrentPopup(IWindow currentWindow, IWindow nextWindow) {
			if (currentWindow != null) {
				var isCurrentWindowPopUp = currentWindow is IPopUp;
				var isNextWindowPopup = nextWindow is IPopUp;
				if (isCurrentWindowPopUp || !isNextWindowPopup) {
					currentWindow.OnHide();
					// Если был попап и следующее окно не попап, закрываем заднее окно
					if (isCurrentWindowPopUp && !isNextWindowPopup) {
						var firstWindow = GetFirstWindow();
						firstWindow.OnHide();
					}
				}
			}
		}

		public void OnRoot() {
			var currentWindow = _stack.Pop();
			currentWindow.OnHide();

#if DEBUG
			D.Warning("[WindowsController.OnRoot]", currentWindow.Name);
#endif
			// Фикс когда попап вызывает рут, если открыт попап, надо закрыть ещё окно под ним
			if (currentWindow is IPopUp) {
				var firstWindow = GetFirstWindow();
				// Если это основное окно, то его не надо закрывать, надо только обновить, код ниже сделает это
				if (!(firstWindow is IMainWindow))
					firstWindow?.OnHide();
			}

			_stack.Clear();

			if (_queue.Count > 0) {
				var (name, argument) = _queue.Dequeue();
				OpenWindow(name, EOpenWindowType.Default, argument);
				return;
			}

			OnChangeWindow(_mainWindow);
			_mainWindow.OnShow();
			_stack.Push(_mainWindow);
		}

		public void OnBack() {
			var currentWindow = _stack.Pop();
			currentWindow.OnHide();

#if DEBUG
			D.Warning("[WindowsController.OnBack.CurrentWindow]", currentWindow.Name);
#endif
			if (_queue.Count > 0) {
				var (name, argument) = _queue.Dequeue();
				OpenWindow(name, EOpenWindowType.Default, argument);
				return;
			}

			if (_stack.Count > 0) {
				var newWindow = _stack.Peek();
				OnBackWindow(newWindow);
				return;
			}

			OnChangeWindow(_mainWindow);
			_mainWindow.OnShow();
			_stack.Push(_mainWindow);
		}

		private void OnBackWindow(IWindow newWindow) {
			if (newWindow is IPopUp) {
				var firstWindow = GetFirstWindow();
				firstWindow?.OnShow();
			}

			OnChangeWindow(newWindow);
			newWindow.OnShow();
#if DEBUG
			D.Warning("[WindowsController.OnBack.NewWindow]", newWindow.Name);
#endif
		}

		public IWindow GetWindow(EWindowName name) {
			if (_activatedWindows.ContainsKey(name))
				return _activatedWindows[name];

			if (!_windows.ContainsKey(name))
				throw new Exception($"Window with name {name} don't found");

			var window = _windows[name];
			window.Initialize();

			_activatedWindows[name] = window;
			return window;
		}

		private IWindow GetFirstWindow() {
			foreach (var element in _stack)
				if (!(element is IPopUp))
					return element;
			return null;
		}

		// Решение круговой зависимости
		public void SubscribeWindowChange(Action<IWindow> callback) => _onWindowChange = callback;

		private void OnChangeWindow(IWindow window) => _onWindowChange?.Invoke(window);
	}
}