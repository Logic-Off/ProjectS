using System;
using System.Collections.Generic;
using Ecs.Ability;
using Ecs.AI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utopia;
using Zentitas;

namespace Ecs.Game {
	[Game]
	public sealed class TeamComponent : IComponent {
		public ETeam Value;
	}

	[Game]
	public sealed class HostileTeamsComponent : IComponent, IDisposable {
		public List<ETeam> Values;
		public void Dispose() => Values.Clear();
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
	public sealed class AbilitiesComponent : IComponent, IDisposable {
		public List<AbilityId> Values;
		public void Dispose() => Values.Clear();
	}

	[Game]
	public sealed class FsmComponent : IComponent { }

	[Game]
	public sealed class PreviousAiAction : IComponent {
		public EAiAction Value;
	}

	[Game]
	public sealed class ItemTransformPositions : IComponent, IDisposable {
		public Dictionary<EItemPosition, Transform> Values;
		public void Dispose() => Values.Clear();
	}

	[Game]
	public sealed class CurrentItemsComponent : IComponent, IDisposable {
		public Dictionary<EItemPosition, AsyncOperationHandle<GameObject>> Values;

		public void Dispose() {
			foreach (var (_, handle) in Values)
				Addressables.ReleaseInstance(handle);
			Values.Clear();
		}
	}

	[Game]
	public sealed class ChangeItemsComponent : IComponent, IDisposable {
		public Dictionary<EItemPosition, string> Values;

		public void Dispose() => Values.Clear();
	}
}