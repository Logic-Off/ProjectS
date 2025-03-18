using System;

namespace Ecs.Character {
	[Serializable]
	public struct CharacterParameters {
		public Parameter Health;
		public Parameter Attack;
		public Parameter CastSpeed;
		public Parameter MovementSpeed;
		public Parameter Observation;
		public Parameter Stealth;
		public Parameter VisionRange;
		public Parameter VisionAngle;

		// Status
		public Parameter Hunger;
		public Parameter Thirst;
		public Parameter Psyche;
		public Parameter Cold;
		public Parameter Radiation;
		
		// Resistance
		public Parameter ResistanceNormalDamage;
		public Parameter ResistancePenetratingDamage;
		public Parameter ResistanceCrushingDamage;
	}
}