using Ecs.Character;
using Utopia;

namespace Ecs.Animations.Commands {
	[InstallerGenerator(InstallerId.Game)]
	public class DealDamageAnimationCommand : IAnimationEventCommand {
		private readonly CharacterContext _character;
		private readonly DamageHandler _damageHandler;
		private readonly ItemContext _item;
		private readonly GameContext _game;

		public DealDamageAnimationCommand(CharacterContext character, DamageHandler damageHandler, ItemContext item, GameContext game) {
			_character = character;
			_damageHandler = damageHandler;
			_item = item;
			_game = game;
		}

		public EAnimationEvent Name => EAnimationEvent.DealDamage;
		public bool Accept(AnimationEventEntity command) => command.HasTarget;

		public void Apply(AnimationEventEntity animationEvent) {
			var owner = _character.GetEntityWithId(animationEvent.Owner.Value);
			var target = _character.GetEntityWithId(animationEvent.Target.Value);
			if (owner == null || target == null)
				return;
			var agent = _game.GetEntityWithId(animationEvent.Owner.Value);
			var damageType = EDamageType.Normal;
			if (agent.HasCurrentWeapon) {
				var currentWeapon = _item.GetEntityWithItemInstanceId(agent.CurrentWeapon.Value);
				damageType = currentWeapon.DamageType.Value;
			}

			var damage = _damageHandler.Damage(target, new DamageData {Type = damageType, Amount = owner.Attack.Value});
			var modifier = new StatModifier(ECharacterStat.Health, EStatModifierType.Damage, damage);
			target.AddModifier(modifier);
		}
	}
}