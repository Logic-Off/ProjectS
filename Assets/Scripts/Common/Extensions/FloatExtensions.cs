namespace Common {
	public static class FloatExtensions {
		public static float Max(this float a, float b) => a > b ? a : b;
		public static float Min(this float a, float b) => a < b ? a : b;
		public static float ToFloat(this double self) => (float) self;
		public static int ToInt(this float self) => (int) self;
		public static long ToLong(this float self) => (long) self;

		public static float Clamp01(this float value) {
			if (value < 0.0f)
				return 0.0f;
			return value > 1.0f ? 1f : value;
		}
	}
}