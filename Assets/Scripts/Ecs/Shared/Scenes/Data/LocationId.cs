using System;
using UnityEngine;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace Ecs.Shared {
	[Serializable]
	public partial struct LocationId : IEquatable<LocationId> {
		[SerializeField] private string _value;

		public LocationId(string value) => _value = value;

		public bool Equals(LocationId other) => _value == other._value;

		public override bool Equals(object obj) => obj is LocationId id && Equals(id);

		public override int GetHashCode() => _value.GetHashCode();

		public static explicit operator LocationId(string value) => new LocationId(value);

		public static implicit operator string(LocationId id) => id._value;

		public static bool operator ==(LocationId a, LocationId b) => a._value == b._value;

		public static bool operator !=(LocationId a, LocationId b) => a._value != b._value;

		public override string ToString() => $"{_value}";
	}

	public partial struct LocationId {
		public static LocationId None = new(string.Empty);
		public static LocationId Base = new("Base");
	}
}