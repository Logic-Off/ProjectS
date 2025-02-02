namespace Common {
	public sealed class EventProperty<T> : Signal<T>, IEventProperty<T> {
		private T _value;

		public EventProperty() { }

		public EventProperty(T value) => _value = value;

		public T Value {
			get => _value;
			set {
				_value = value;
				Fire();
			}
		}

		public void Fire() {
			foreach (var listener in Listeners)
				listener.Invoke(Value);
		}

		/// <summary>
		/// Сохраняет значение и уведомляет подписчиков 
		/// </summary>
		/// <param name="value"></param>
		public override void Fire(T value) => Value = value;
	}
}