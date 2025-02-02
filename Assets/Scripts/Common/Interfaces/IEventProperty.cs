namespace Common {
	public interface IEventProperty<T> : ISignal<T> {
		T Value { get; set; }
		void Fire();
	}
}