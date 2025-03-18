using System;
using UnityEngine;

// ReSharper disable NonReadonlyMemberInGetHashCode
namespace Ecs.Item {
	[Serializable]
	public partial struct ItemId : IEquatable<ItemId> {
		[SerializeField] private string _value;

		public ItemId(string value) => _value = value;

		public bool Equals(ItemId other) => _value == other._value;

		public override bool Equals(object obj) => obj is ItemId id && Equals(id);

		public override int GetHashCode() => _value.GetHashCode();

		public static explicit operator ItemId(string value) => new ItemId(value);

		public static implicit operator string(ItemId id) => id._value;

		public static bool operator ==(ItemId a, ItemId b) => a._value == b._value;

		public static bool operator !=(ItemId a, ItemId b) => a._value != b._value;

		public override string ToString() => $"{_value}";
	}
}