using System;
using UnityEngine;

namespace Ecs.Ability {
	[Serializable]
	public partial struct AbilityId : IEquatable<AbilityId> {
		[SerializeField] private string _value;

		public AbilityId(string value) => _value = value;

		public bool Equals(AbilityId other) => _value == other._value;

		public override bool Equals(object obj) => obj is AbilityId id && Equals(id);

		public override int GetHashCode() => _value.GetHashCode();

		public static explicit operator AbilityId(string value) => new AbilityId(value);

		public static implicit operator string(AbilityId id) => id._value;

		public static bool operator ==(AbilityId a, AbilityId b) => a._value == b._value;

		public static bool operator !=(AbilityId a, AbilityId b) => a._value != b._value;

		public override string ToString() => $"{_value}";
	}
}