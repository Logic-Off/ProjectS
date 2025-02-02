using System;

namespace Common {
	public interface ISignal : IDisposable {
		void Fire();
		void AddListener(Action listener);
		void RemoveListener(Action listener);
		void ClearListeners();
	}

	public interface ISignal<T> : IDisposable {
		void Fire(T value);
		void AddListener(Action<T> listener);
		void RemoveListener(Action<T> listener);
		void ClearListeners();
	}

	public interface ISignal<T, V> : IDisposable {
		void Fire(T tValue, V vValue);
		void AddListener(Action<T, V> listener);
		void RemoveListener(Action<T, V> listener);
		void ClearListeners();
	}
}