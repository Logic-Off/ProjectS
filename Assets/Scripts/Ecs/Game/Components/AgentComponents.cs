using System;
using System.Collections.Generic;
using Ecs.Ability;
using Ecs.Character;
using Ecs.Common;
using UnityEngine;
using Utopia;
using Zentitas;

namespace Ecs.Game {
	[Game]
	public sealed class TeamComponent : IComponent {
		public ETeam Value;
	}

	[Game]
	public sealed class HostileTeamsComponent : IComponent {
		public List<ETeam> Values;
	}

	[Game]
	public sealed class CurrentAnimationStateComponent : IComponent {
		public string Value;
	}

	[Game]
	public sealed class PreviousAnimationStateComponent : IComponent {
		public string Value;
	}

	[Game]
	public sealed class AttackTargetComponent : IComponent {
		public TargetData Value;
	}

	[Game]
	public sealed class LastAttackTargetComponent : IComponent {
		public TargetData Value;
	}

	[Game]
	public sealed class AttackTargetsComponent : IComponent, IDisposable {
		public List<TargetData> Values;

		public void Dispose() => Values.Clear();
	}

	[Game]
	public sealed class HostileTargetComponent : IComponent {
		public TargetData Value;
	}

	[Game]
	public sealed class LastHostileTargetComponent : IComponent {
		public TargetData Value;
	}

	[Game]
	public sealed class HostileTargetsComponent : IComponent, IDisposable {
		public List<TargetData> Values;

		public void Dispose() => Values.Clear();
	}

	[Game]
	public sealed class DeadComponent : IComponent { }

	[Game]
	public sealed class LayerMaskComponent : IComponent {
		public LayerMask Value;
		public int MaskIndex;
	}

	[Game, Event(InstallerId.Game, EEventType.AddedOrRemoved, 100_00, true, false)]
	public sealed class PlayerTargetComponent : IComponent {
		public override string ToString() => $"PlayerTarget";
	}

	[Game]
	public sealed class AbilitiesComponent : IComponent {
		public List<AbilityId> Values;
	}

	[Game]
	public sealed class FsmComponent : IComponent { }
}