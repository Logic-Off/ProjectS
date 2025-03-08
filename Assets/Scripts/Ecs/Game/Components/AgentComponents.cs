using System.Collections.Generic;
using Ecs.Character;
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
}