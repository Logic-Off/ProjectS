namespace Common {
	public static class CommonExtensions {
		public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);
		public static bool IsNotNullOrEmpty(this string value) => !string.IsNullOrEmpty(value);
	}
}