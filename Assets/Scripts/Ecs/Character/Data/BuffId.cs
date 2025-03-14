using System;
using UnityEngine;

namespace Ecs.Character {
	[Serializable]
	public partial struct BuffId : IEquatable<BuffId> {
		[SerializeField] private string _value;

		public BuffId(string value) => _value = value;

		public bool Equals(BuffId other) => _value == other._value;

		public override bool Equals(object obj) => obj is BuffId id && Equals(id);

		public override int GetHashCode() => _value.GetHashCode();

		public static explicit operator BuffId(string value) => new BuffId(value);

		public static implicit operator string(BuffId id) => id._value;

		public static bool operator ==(BuffId a, BuffId b) => a._value == b._value;

		public static bool operator !=(BuffId a, BuffId b) => a._value != b._value;

		public override string ToString() => $"{_value}";
	}
}