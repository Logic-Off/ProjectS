using System;

namespace Ecs.Character {
	[Serializable]
	public struct Parameter {
		public float Amount;
		public float Add;
		public float Multiply;
		public float Value => Amount * Multiply + Add;

		public Parameter(float amount) {
			Amount = amount;
			Add = 0;
			Multiply = 1;
		}

		public Parameter(float amount, float multiply) {
			Amount = amount;
			Add = 0;
			Multiply = 1f + multiply;
		}

		public static float operator +(float a, Parameter b) => (new Parameter(a) + b).Value;
		public static float operator -(float a, Parameter b) => (new Parameter(a) - b).Value;

		public static Parameter operator +(Parameter a, Parameter b) =>
			new() {
				Add = a.Add + b.Add,
				Multiply = a.Multiply + b.Multiply - 1f,
				Amount = a.Amount + b.Amount
			};

		public static Parameter operator *(Parameter a, Parameter b) =>
			new() {
				Add = a.Add + b.Add,
				Multiply = a.Multiply * b.Multiply,
				Amount = a.Amount + b.Amount
			};

		public static Parameter operator -(Parameter a, Parameter b) =>
			new() {
				Add = Math.Max(a.Add - b.Add, 0),
				Multiply = 1f + a.Multiply - b.Multiply,
				Amount = Math.Max(a.Amount - b.Amount, 0)
			};

		public override string ToString() => $"{Value:F2} = {Amount:F2} * {Multiply:F2} + {Add:F2}";
	}
}