using System;

namespace Ecs.Inventory {
	[Serializable]
	public struct ContainerId : IEquatable<ContainerId> {
		public static ContainerId None = new(-1);
		private readonly int _value;

		public ContainerId(int value) => _value = value;

		public bool Equals(ContainerId other) => _value == other._value;

		public override bool Equals(object obj) => obj is ContainerId && Equals((ContainerId)obj);

		public override int GetHashCode() => _value;

		public static explicit operator ContainerId(int value) => new ContainerId(value);

		public static implicit operator int(ContainerId id) => id._value;

		public static bool operator ==(ContainerId a, ContainerId b) => a._value == b._value;

		public static bool operator !=(ContainerId a, ContainerId b) => a._value != b._value;

		public override string ToString() =>  $"Container.{_value}";
	}
}