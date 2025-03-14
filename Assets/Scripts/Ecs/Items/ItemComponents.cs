using System.Collections.Generic;
using Ecs.Ability;
using Ecs.Character;
using Ecs.Inventory;
using Zentitas;

namespace Ecs.Item {
	[Item]
	public sealed class ItemIdComponent : IComponent {
		public ItemId Value;
	}

	[Item]
	public sealed class QuantityComponent : IComponent {
		public int Value;
	}

	[Item]
	public sealed class StackSizeComponent : IComponent {
		public int Value;
	}

	[Item]
	public sealed class ItemInstanceIdComponent : IComponent {
		[PrimaryEntityIndex] public ItemInstanceId Value;
	}

	[Item]
	public sealed class ItemTypeComponent : IComponent {
		public EItemType Value;
	}

	[Item]
	public sealed class StackableComponent : IComponent {
	}

	[Item, Inventory, Structure]
	public sealed class CellSettingsComponent : IComponent {
		public Dictionary<ECellType, int> Values;
	}

	[Item]
	public sealed class DurabilityComponent : IComponent {
		public float Value;
	}

	[Item]
	public sealed class TargetBuffsComponent : IComponent {
		public List<BuffData> Values;
	}

	[Item]
	public sealed class DamageTypeComponent : IComponent {
		public EDamageType Value;
	}

	[Item]
	public sealed class MaxDurabilityComponent : IComponent {
		public float Value;
	}

	[Item]
	public sealed class BrokenComponent : IComponent {
	}

	[Item]
	public sealed class ToolTypeComponent : IComponent {
		public EToolType Value;
	}
}