using System;
using UnityEngine;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace Ecs.Item {
	[Serializable]
	public partial struct CellSettingsId : IEquatable<CellSettingsId> {
		[SerializeField] private string _value;

		public CellSettingsId(string value) => _value = value;

		public bool Equals(CellSettingsId other) => _value == other._value;

		public override bool Equals(object obj) => obj is CellSettingsId id && Equals(id);

		public override int GetHashCode() => _value.GetHashCode();

		public static explicit operator CellSettingsId(string value) => new CellSettingsId(value);

		public static implicit operator string(CellSettingsId id) => id._value;

		public static bool operator ==(CellSettingsId a, CellSettingsId b) => a._value == b._value;

		public static bool operator !=(CellSettingsId a, CellSettingsId b) => a._value != b._value;

		public override string ToString() => $"{_value}";
	}
}