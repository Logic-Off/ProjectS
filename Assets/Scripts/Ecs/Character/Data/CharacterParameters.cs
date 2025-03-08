using System;

namespace Ecs.Character {
	[Serializable]
	public struct CharacterParameters {
		public Parameter Health;
		public Parameter Attack;
		public Parameter CastSpeed;
		public Parameter MovementSpeed;
	}
}