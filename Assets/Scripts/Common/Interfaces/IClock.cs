namespace Common {
	public interface IClock {
		float Time { get; }
		long Timestamp { get; }
	}
}