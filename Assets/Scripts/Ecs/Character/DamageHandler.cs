using System;
using Common;
using Utopia;

namespace Ecs.Character {
	[InstallerGenerator(InstallerId.Game)]
	public class DamageHandler {
		public float Damage(CharacterEntity target, DamageData data) {
			var damage = 0f;
			switch (data.Type) {
				case EDamageType.Normal:
					damage = data.Amount - (data.Amount * target.ResistanceNormalDamage.Value);
					break;
				case EDamageType.Piercing:
					damage = data.Amount - (data.Amount * target.ResistancePenetratingDamage.Value);
					break;
				case EDamageType.Crushing:
					damage = data.Amount - (data.Amount * target.ResistanceCrushingDamage.Value);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			return -damage.Max(0);
		}
	}
}