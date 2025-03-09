using System;
using UnityEngine;

namespace Ecs.Ability {
	[Serializable]
	public struct AbilityComponentId : IEquatable<AbilityComponentId> {
		[SerializeField] private string _value;

		public AbilityComponentId(string value) => _value = value;

		public bool Equals(AbilityComponentId other) => _value == other._value;

		public override bool Equals(object obj) => obj is AbilityComponentId id && Equals(id);

		public override int GetHashCode() => _value.GetHashCode();

		public static explicit operator AbilityComponentId(string value) => new AbilityComponentId(value);

		public static implicit operator string(AbilityComponentId id) => id._value;

		public static bool operator ==(AbilityComponentId a, AbilityComponentId b) => a._value == b._value;

		public static bool operator !=(AbilityComponentId a, AbilityComponentId b) => a._value != b._value;

		public override string ToString() => _value;
	}
}