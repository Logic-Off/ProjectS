using Ecs.Character;
using Utopia;

namespace Ecs.Animations.Commands {
	[InstallerGenerator(InstallerId.Game)]
	public class DealDamageAnimationCommand : IAnimationEventCommand {
		private readonly CharacterContext _character;
		private readonly DamageHandler _damageHandler;

		public DealDamageAnimationCommand(CharacterContext character, DamageHandler damageHandler) {
			_character = character;
			_damageHandler = damageHandler;
		}

		public EAnimationEvent Name => EAnimationEvent.DealDamage;
		public bool Accept(AnimationEventEntity command) => command.HasTarget;

		public void Apply(AnimationEventEntity animationEvent) {
			var owner = _character.GetEntityWithId(animationEvent.Owner.Value);
			var target = _character.GetEntityWithId(animationEvent.Target.Value);
			if (owner == null || target == null)
				return;

			var damage = _damageHandler.Damage(target, new DamageData {Type = EDamageType.Penetrating, Amount = owner.Attack.Value});
			var modifier = new StatModifier(ECharacterStat.Health, EStatModifierType.Damage, damage);
			target.AddModifier(modifier);
		}
	}
}