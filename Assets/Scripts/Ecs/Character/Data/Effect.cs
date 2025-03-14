using System;

namespace Ecs.Character {
	[Serializable]
	public struct Effect {
		public ECharacterStat Type;
		public Parameter Parameter;

		public Effect(ECharacterStat type, Parameter parameter) {
			Type = type;
			Parameter = parameter;
		}
	}
}