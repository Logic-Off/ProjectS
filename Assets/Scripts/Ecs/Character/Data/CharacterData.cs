using System;
using System.Collections.Generic;

namespace Ecs.Character {
	[Serializable]
	public struct CharacterData {
		public string Name;
		public ETeam Team;
		public List<ETeam> HostileTeams;
		public List<CharacterParametersData> Levels;
		public string BaseAnimationState;
	}
}