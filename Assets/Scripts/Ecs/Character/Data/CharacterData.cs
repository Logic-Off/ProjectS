using System;
using System.Collections.Generic;
using Ecs.Ability;
using Ecs.AI;
using Ecs.Game;

namespace Ecs.Character {
	[Serializable]
	public struct CharacterData {
		public string Name;
		public ETeam Team;
		public List<ETeam> HostileTeams;
		public List<CharacterParametersData> Levels;
		public string BaseAnimationState;
		public AbilityId BaseAbility;
		public List<AbilityId> Abilities;
		public List<EAiAction> Actions;
	}
}