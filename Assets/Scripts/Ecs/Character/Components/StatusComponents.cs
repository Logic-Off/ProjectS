using System;
using System.Collections.Generic;
using Zentitas;

namespace Ecs.Character {
	[Character]
	public sealed class HungerComponent : IComponent {
		public float Value;
	}

	[Character]
	public sealed class MaxHungerComponent : IComponent {
		public float Value;
	}

	[Character]
	public sealed class ThirstComponent : IComponent {
		public float Value;
	}

	[Character]
	public sealed class MaxThirstComponent : IComponent {
		public float Value;
	}

	[Character]
	public sealed class PsycheComponent : IComponent {
		public float Value;
	}

	[Character]
	public sealed class MaxPsycheComponent : IComponent {
		public float Value;
	}

	[Character]
	public sealed class ColdComponent : IComponent {
		public float Value;
	}

	[Character]
	public sealed class MaxColdComponent : IComponent {
		public float Value;
	}

	[Character]
	public sealed class RadiationComponent : IComponent {
		public float Value;
	}

	[Character]
	public sealed class MaxRadiationComponent : IComponent {
		public float Value;
	}
}