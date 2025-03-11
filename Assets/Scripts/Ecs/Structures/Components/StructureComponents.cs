using System.Collections.Generic;
using Ecs.Game;
using Zentitas;

namespace Ecs.Structures {
	[Structure]
	public sealed class StructureTypeComponent : IComponent {
		[EntityIndex] public EStructureType Value;
	}

	[Structure]
	public sealed class SpawnPointComponent : IComponent {
		[EntityIndex] public ESpawnPointType Value;
	}

	[Structure]
	public sealed class TeamsComponent : IComponent {
		public List<ETeam> Values;
	}
}