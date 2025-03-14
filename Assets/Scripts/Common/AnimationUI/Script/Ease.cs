namespace Common {
	public static class Ease {
		public static float InQuint(float x) => x * x * x * x * x;
		public static float OutQuint(float x) => -((1 - x) * (1 - x) * (1 - x) * (1 - x) * (1 - x)) + 1;

		public static float InOutQuint(float x) => x < 0.5
			? 8 * x * x * x * x * x
			: 1 - ((-2 * x + 2) * (-2 * x + 2) * (-2 * x + 2) * (-2 * x + 2) * (-2 * x + 2)) / 2;

		public static float OutBackQuint(float x) => -(x - 1) * (x - 1) * (x - 1) * (x - 1) + 5 * (x - 1) * (x - 1) * (x - 1) + 5 * (x - 1) * (x - 1) + 1;

		public static float InQuart(float x) => x * x * x * x;
		public static float OutQuart(float x) => -((1 - x) * (1 - x) * (1 - x) * (1 - x)) + 1;

		public static float InOutQuart(float x) => x < 0.5
			? 8 * x * x * x * x
			: 1 - ((-2 * x + 2) * (-2 * x + 2) * (-2 * x + 2) * (-2 * x + 2)) / 2;

		public static float OutBackQuart(float x) => -(x - 1) * (x - 1) * (x - 1) * (x - 1) + 4 * (x - 1) * (x - 1) * (x - 1) + 4 * (x - 1) * (x - 1) + 1;

		public static float InCubic(float x) => x * x * x;
		public static float OutCubic(float x) => -((1 - x) * (1 - x) * (1 - x)) + 1;

		public static float InOutCubic(float x) => x < 0.5
			? 4 * x * x * x
			: 1 - ((-2 * x + 2) * (-2 * x + 2) * (-2 * x + 2)) / 2;

		public static float OutBackCubic(float x) => -(x - 1) * (x - 1) * (x - 1) * (x - 1) + 3 * (x - 1) * (x - 1) * (x - 1) + 3 * (x - 1) * (x - 1) + 1;

		public static float InQuad(float x) => x * x;
		public static float OutQuad(float x) => -((1 - x) * (1 - x)) + 1;

		public static float InOutQuad(float x) => x < 0.5
			? 2 * x * x
			: 1 - ((-2 * x + 2) * (-2 * x + 2)) / 2;

		public static float OutBackQuad(float x) => -(x - 1) * (x - 1) * (x - 1) * (x - 1) + 2 * (x - 1) * (x - 1) * (x - 1) + 2 * (x - 1) * (x - 1) + 1;

		public static float Linear(float x) => x;

		public static float OutBackLinear(float x) => -(x - 1) * (x - 1) * (x - 1) * (x - 1) + (x - 1) * (x - 1) * (x - 1) + (x - 1) * (x - 1) + 1;

		public static float OutPowBack(float x, float p) => -(x - 1) * (x - 1) * (x - 1) * (x - 1) + p * (x - 1) * (x - 1) * (x - 1) + p * (x - 1) * (x - 1) + 1;

		public static float OutBack(float x) => 1 + 2.70158f * (x - 1) * (x - 1) * (x - 1) + 1.70158f * (x - 1) * (x - 1);

		// Func<float,float> is longer and harder to type than Ease.Function
		// Also it make sure the kind of function that gets passed are function that has
		// a return float that is normalized between 0 and 1 (can also overshoot but the main part is between 0 and 1)
		public delegate float Function(float x);

		public static Function GetEase(EaseType type, EasePower power) {
			if (power == EasePower.Linear)
				return Linear;

			if (power == EasePower.Quad) {
				return type switch {
					EaseType.In => InQuad,
					EaseType.Out => OutQuad,
					EaseType.InOut => InOutQuad,
					EaseType.OutBack => OutBackQuad,
					_ => Linear
				};
			}

			if (power == EasePower.Cubic) {
				return type switch {
					EaseType.In => InCubic,
					EaseType.Out => OutCubic,
					EaseType.InOut => InOutCubic,
					EaseType.OutBack => OutBackCubic,
					_ => Linear
				};
			}

			if (power == EasePower.Quart) {
				return type switch {
					EaseType.In => InQuart,
					EaseType.Out => OutQuart,
					EaseType.InOut => InOutQuart,
					EaseType.OutBack => OutBackQuart,
					_ => Linear
				};
			}

			if (power == EasePower.Quint) {
				return type switch {
					EaseType.In => InQuint,
					EaseType.Out => OutQuint,
					EaseType.InOut => InOutQuint,
					EaseType.OutBack => OutBackQuint,
					_ => Linear
				};
			}

			return Linear;
		}
	}

	public enum EasePower {
		Linear,
		Quad,
		Cubic,
		Quart,
		Quint
	}

	public enum EaseType {
		In,
		Out,
		InOut,
		OutBack
	}
}