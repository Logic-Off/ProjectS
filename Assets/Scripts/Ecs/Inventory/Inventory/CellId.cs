using System;

namespace Ecs.Inventory {
	[Serializable]
	public struct CellId : IEquatable<CellId> {
		public static CellId None => new CellId(-1);
		private readonly int _value;

		public CellId(int value) {
			_value = value;
		}

		public bool Equals(CellId other) => _value == other._value;

		public override bool Equals(object obj) => obj is CellId && Equals((CellId)obj);

		public override int GetHashCode() => _value;

		public static explicit operator CellId(int value) => new CellId(value);

		public static implicit operator int(CellId id) => id._value;

		public static bool operator ==(CellId a, CellId b) => a._value == b._value;

		public static bool operator !=(CellId a, CellId b) => a._value != b._value;

		public override string ToString() => $"Cell.{_value}";
	}
}