using System;

namespace Ecs.Inventory {
	[Serializable]
	public struct ItemInstanceId : IEquatable<ItemInstanceId> {
		public static ItemInstanceId None => new(-1);
		private readonly int _value;

		public ItemInstanceId(int value) => _value = value;

		public bool Equals(ItemInstanceId other) => _value == other._value;

		public override bool Equals(object obj) => obj is ItemInstanceId && Equals((ItemInstanceId)obj);

		public override int GetHashCode() => _value;

		public static explicit operator ItemInstanceId(int value) => new ItemInstanceId(value);

		public static implicit operator int(ItemInstanceId id) => id._value;

		public static bool operator ==(ItemInstanceId a, ItemInstanceId b) => a._value == b._value;

		public static bool operator !=(ItemInstanceId a, ItemInstanceId b) => a._value != b._value;

		public override string ToString() => $"ItemInstance.{_value}";
	}
}