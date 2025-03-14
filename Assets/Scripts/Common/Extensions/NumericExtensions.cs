namespace Common {
	public static class NumericExtensions {
		public static int Max(this int a, int b) => a > b ? a : b;
		public static int Min(this int a, int b) => a < b ? a : b;
		public static float Max(this float a, float b) => a > b ? a : b;
		public static float Min(this float a, float b) => a < b ? a : b;

		public static float Clamp(this float num, float min, float max) {
			if (num < min)
				return min;
			if (num > max)
				return max;
			return num;
		}

		public static float ToFloat(this double self) => (float) self;
		public static float ToFloat(this int self) => self;
		public static int ToInt(this float self) => (int) self;
		public static long ToLong(this float self) => (long) self;

		public static float Clamp01(this float value) {
			if (value < 0.0f)
				return 0.0f;
			return value > 1.0f ? 1f : value;
		}
	}
}