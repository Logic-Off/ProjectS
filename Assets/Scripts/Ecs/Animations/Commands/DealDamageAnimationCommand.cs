using Ecs.Character;
using Utopia;

namespace Ecs.Animations.Commands {
	[InstallerGenerator(InstallerId.Game)]
	public class DealDamageAnimationCommand : IAnimationEventCommand {
		private readonly CharacterContext _character;

		public DealDamageAnimationCommand(CharacterContext character) => _character = character;

		public EAnimationEvent Name => EAnimationEvent.DealDamage;
		public bool Accept(AnimationEventEntity entity) => entity.HasTarget;

		public void Apply(AnimationEventEntity animationEvent) {
			var owner = _character.GetEntityWithId(animationEvent.Owner.Value);
			var target = _character.GetEntityWithId(animationEvent.Target.Value);
			if (owner == null || target == null)
				return;

			var modifier = new StatModifier() {
				Type = EStatModifierType.Damage,
				Value = -owner.Attack.Value
			};

			target.AddModifier(ECharacterStat.Health, modifier);
		}
	}
}