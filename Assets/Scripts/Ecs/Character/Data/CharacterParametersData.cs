using System;

namespace Ecs.Character {
	[Serializable]
	public struct CharacterParametersData {
		public float Health;
		public float Attack;
		public float CastSpeed;
		public float MovementSpeed;
		public float VisionRange;
		public float VisionAngle;
		
		// Status
		public float Hunger;
		public float Thirst;
		public float Psyche;
		public float Cold;
		public float Radiation;
		
		// Resistance
		public float ResistanceNormalDamage;
		public float ResistancePiercingDamage;
		public float ResistanceCrushingDamage;
		public float ResistanceSlashingDamage;

		public CharacterParameters ToParameters => new() {
			Health = new Parameter(Health),
			Attack = new Parameter(Attack),
			CastSpeed = new Parameter(CastSpeed),
			MovementSpeed = new Parameter(MovementSpeed),
			VisionRange = new Parameter(VisionRange),
			VisionAngle = new Parameter(VisionAngle),
			Hunger = new Parameter(Hunger),
			Thirst = new Parameter(Thirst),
			Psyche = new Parameter(Psyche),
			Cold = new Parameter(Cold),
			Radiation = new Parameter(Radiation),
			ResistanceNormalDamage = new Parameter(ResistanceNormalDamage),
			ResistancePiercingDamage = new Parameter(ResistancePiercingDamage),
			ResistanceCrushingDamage = new Parameter(ResistanceCrushingDamage),
			ResistanceSlashingDamage = new Parameter(ResistanceSlashingDamage),
		};
	}
}