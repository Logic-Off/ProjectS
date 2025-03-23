using System;
using System.Collections.Generic;
using Common;
using Ecs.Inventory;
using Ui;
using UnityEngine;
using Utopia;
using Zentitas;

namespace Ecs.Ui {
	[Ui]
	public sealed class UiTypeComponent : IComponent {
		[EntityIndex] public EUiType Value;
	}

	[Ui]
	public sealed class WindowNameComponent : IComponent {
		[PrimaryEntityIndex] public EWindowName Value;
	}

	[Ui]
	public sealed class PanelNameComponent : IComponent {
		[PrimaryEntityIndex] public EPanelName Value;
	}

	[Ui, Event(InstallerId.Ui, EEventType.AddedOrRemoved, 750_000, false, true)]
	public sealed class ActiveComponent : IComponent { }

	[Ui, Game, Event(InstallerId.Ui, EEventType.AddedOrRemoved, 750_000, false, true)]
	public sealed class VisibleComponent : IComponent { }

	[Ui, Event(InstallerId.Ui, EEventType.Added)]
	public sealed class SiblingComponent : IComponent {
		public ESibling Value;
	}

	[Ui]
	public sealed class RectComponent : IComponent {
		public RectTransform Value;
	}

	[Ui]
	public sealed class AnimatedComponent : IComponent { }

	[Ui, Event(InstallerId.Ui, EEventType.Added, 500_000, false, true)]
	public sealed class Vector2Component : IComponent {
		public Vector2 Value;
	}

	[Ui, Event(InstallerId.Ui, EEventType.Added, 500_000, false, true)]
	public sealed class ClickedComponent : IComponent { }

	[Ui]
	public sealed class IconIdComponent : IComponent {
		public string Value;
	}

	[Ui, Event(InstallerId.Ui, EEventType.Added)]
	public sealed class SpriteComponent : IComponent {
		public Sprite Value;
	}

	[Ui]
	public sealed class TargetCellIdComponent : IComponent {
		[EntityIndex] public CellId Value;
	}

	[Ui]
	public sealed class TargetContainerIdComponent : IComponent {
		[EntityIndex] public ContainerId Value;
	}

	[Ui, Event(InstallerId.Ui, EEventType.Added)]
	public sealed class IntComponent : IComponent {
		public int Value;
	}

	[Ui, Event(InstallerId.Ui, EEventType.Added)]
	public sealed class FloatComponent : IComponent {
		public float Value;
	}

	[Ui, Event(InstallerId.Ui, EEventType.Added)]
	public sealed class StringComponent : IComponent {
		public string Value;
	}

	[Ui, Event(InstallerId.Ui, EEventType.Added)]
	public sealed class ColorComponent : IComponent {
		public Color Value;
	}

	[Ui, Event(InstallerId.Ui, EEventType.AddedOrRemoved)]
	public sealed class InteractableComponent : IComponent { }

	[Ui, Event(InstallerId.Ui, EEventType.AddedOrRemoved, 750_000, false)]
	public sealed class SelectedComponent : IComponent { }

	[Ui]
	public sealed class TouchEventsComponent : IComponent {
		public List<TouchEvent> List;
	}

	[Ui]
	public sealed class OnTouchSubscribersComponent : IComponent {
		public List<Action<UiEntity, TouchEvent>> List;
	}

	[Ui, Event(InstallerId.Ui, EEventType.AddedOrRemoved, 750_000, false, true)]
	public sealed class PressedComponent : IComponent { }
	
	
	[Ui, Event(InstallerId.Ui, EEventType.Added)]
	public sealed class LocalizationComponent : IComponent {
		public LocalizationKey Value;
		public override string ToString() => $"LocalizationKeys[{Value.Key}]";
	}
}