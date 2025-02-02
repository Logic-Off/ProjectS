using System;
using System.Collections.Generic;

namespace Common {
	public class Signal : ISignal {
		private readonly List<Action> _listeners = new();
		protected List<Action> Listeners => _listeners;

		public void Fire() {
			foreach (var listener in Listeners)
				listener.Invoke();
		}

		public void AddListener(Action listener) => Listeners.Add(listener);

		public void RemoveListener(Action listener) => Listeners.Remove(listener);

		public void ClearListeners() => Listeners.Clear();

		public virtual void Dispose() => Listeners.Clear();
	}

	public class Signal<T> : ISignal<T> {
		private readonly List<Action<T>> _listeners = new();
		protected List<Action<T>> Listeners => _listeners;

		public virtual void Fire(T value) {
			foreach (var listener in Listeners)
				listener.Invoke(value);
		}

		public void AddListener(Action<T> listener) => Listeners.Add(listener);

		public void RemoveListener(Action<T> listener) => Listeners.Remove(listener);

		public void ClearListeners() => Listeners.Clear();

		public virtual void Dispose() => Listeners.Clear();
	}

	public class Signal<T, V> : ISignal<T, V> {
		private readonly List<Action<T, V>> _listeners = new();
		protected List<Action<T, V>> Listeners => _listeners;

		public virtual void Fire(T tValue, V vValue) {
			foreach (var listener in Listeners)
				listener.Invoke(tValue, vValue);
		}

		public void AddListener(Action<T, V> listener) => Listeners.Add(listener);

		public void RemoveListener(Action<T, V> listener) => Listeners.Remove(listener);

		public void ClearListeners() => Listeners.Clear();

		public void Dispose() => Listeners.Clear();
	}
}