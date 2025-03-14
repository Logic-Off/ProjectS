using System;
using System.Collections.Generic;
using Ecs.Item;
using Zentitas;

namespace Ecs.Inventory {
	[Inventory]
	public sealed class ChangedComponent : IComponent { }

	[Inventory]
	public sealed class CellsPoolComponent : IComponent, IDisposable {
		public List<CellId> Value;
		public void Dispose() => Value.Clear();
	}

	[Inventory]
	public sealed class CellsComponent : IComponent, IDisposable {
		public List<CellId> Value;
		public void Dispose() => Value.Clear();
	}

	[Inventory, Ui]
	public sealed class ContainerTypeComponent : IComponent {
		public EContainerType Value;
	}

	[Inventory, Item]
	public sealed class CellIdComponent : IComponent {
		[PrimaryEntityIndex] public CellId Value;
	}

	[Inventory]
	public sealed class ContainerIdComponent : IComponent {
		[PrimaryEntityIndex] public ContainerId Value;
	}

	[Inventory, Item]
	public sealed class ContainerOwnerComponent : IComponent {
		[EntityIndex] public ContainerId Value;
	}

	[Inventory, Ui]
	public sealed class EmptyComponent : IComponent { }

	[Inventory]
	public sealed class BrokenComponent : IComponent { }

	[Inventory]
	public sealed class ItemTypesComponent : IComponent, IDisposable {
		public List<EItemType> Value;
		public void Dispose() => Value.Clear();
	}

	[Inventory]
	public sealed class ActiveContainerComponent : IComponent { }

	[Inventory]
	public sealed class CellTargetComponent : IComponent {
		public ItemInstanceId Value;
	}

	[Shared, Unique]
	public sealed class InventoryComponent : IComponent {
		public Dictionary<ItemId, int> Values;
	}

	[Shared, Unique]
	public sealed class PlayerInventoryComponent : IComponent, IDisposable {
		public List<ContainerSave> Values;

		public void Dispose() => Values.Clear();
	}

	[Shared, Unique]
	public sealed class StorageComponent : IComponent, IDisposable {
		public List<StorageEntry> Values;
		public void Dispose() => Values.Clear();
	}

	[Serializable]
	public struct ContainerSave {
		public EContainerType Type;
		public Dictionary<ECellType, int> CellSettings;
		public List<CellSaveEntry> Cells;

		public ContainerSave(EContainerType type, Dictionary<ECellType, int> cellSettings) {
			Type = type;
			CellSettings = cellSettings;
			Cells = new List<CellSaveEntry>();
		}
	}

	[Serializable]
	public struct CellSaveEntry {
		public ItemSaveEntry Item;
		public int Index;
	}

	public struct ItemSaveEntry {
		public ItemId ItemId; // Айди итема
		public float Durability; // Прочность итема
		public int Quantity; // Количество итемов
		public ContainerSave ContainerSave; // Контейнер итема
	}
}