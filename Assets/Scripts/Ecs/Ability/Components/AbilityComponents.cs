using System.Collections.Generic;
using Ecs.Common;
using Zentitas;

namespace Ecs.Ability {
	[Ability]
	public sealed class AbilityIdComponent : IComponent {
		public AbilityId Value;
	}

	[Ability]
	public sealed class AbilityIdCommandComponent : IComponent {
		public AbilityComponentId Value;
	}

	[Ability]
	public sealed class StatusComponent : IComponent {
		public EAbilityStatus Value;
	}

	[Ability]
	public sealed class CommandsComponent : IComponent {
		public List<Id> Values;
	}

	[Ability]
	public sealed class CooldownTimeComponent : IComponent {
		public float Value;
	}

	[Ability]
	public sealed class EndCooldownTimeComponent : IComponent {
		public long Value;
	}

	[Ability]
	public sealed class CastComponent : IComponent { }

	[Ability]
	public sealed class CooldownComponent : IComponent { }

	[Ability]
	public sealed class DurationComponent : IComponent {
		public float Value;
	}

	[Ability]
	public sealed class AbilityTypeComponent : IComponent {
		[EntityIndex] public EAbilityType Value;
	}

	[Ability]
	public sealed class FloatComponent : IComponent {
		public float Value;
	}

	[Ability]
	public sealed class ParametersComponent : IComponent {
		public Dictionary<EAbilityParameter, float> Values;
	}

	[Ability]
	public sealed class AnimationIdComponent : IComponent {
		public string Value;
	}

	[Ability]
	public sealed class StandingCastComponent : IComponent { }
}