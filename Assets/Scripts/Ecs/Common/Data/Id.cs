using System;

namespace Ecs.Common {
	[Serializable]
	public struct Id : IEquatable<Id> {
		public static Id None = new(-1);
		private readonly int _value;

		public Id(int value) => _value = value;

		public bool Equals(Id other) => _value == other._value;

		public override bool Equals(object obj) => obj is Id && Equals((Id)obj);

		public override int GetHashCode() => _value;

		public static explicit operator Id(int value) => new Id(value);

		public static implicit operator int(Id id) => id._value;

		public static bool operator ==(Id a, Id b) => a._value == b._value;

		public static bool operator !=(Id a, Id b) => a._value != b._value;

		public override string ToString() => $"{_value}";
	}
}