using Common;
using Ecs.Ability;
using Ecs.Animations;
using Ecs.Game;
using UnityEngine;
using Utopia;
using Zentitas;
using AnimationEvent = Ecs.Animations.AnimationEvent;

namespace Ecs.Command {
	[InstallerGenerator(InstallerId.Game)]
	public class AbilityCommandState : ICommandState {
		public EState State => EState.Ability;

		private readonly IAnimationsDatabase _animations;
		private readonly IClock _clock;
		private readonly AbilityContext _ability;
		private readonly CharacterContext _character;
		private readonly AbilityStrategy _abilityStrategy;

		public AbilityCommandState(
			IAnimationsDatabase animations,
			IClock clock,
			AbilityContext ability,
			CharacterContext character,
			AbilityStrategy abilityStrategy
		) {
			_animations = animations;
			_clock = clock;
			_ability = ability;
			_character = character;
			_abilityStrategy = abilityStrategy;
		}

		public void Enter(GameEntity agent, CommandEntity command) {
			var ability = _ability.GetEntityWithId(command.Ability.Value);
			OnAbility(ability);
			OnAnimation(agent, command, ability.AnimationId.Value);
		}

		private void OnAbility(AbilityEntity ability) => _abilityStrategy.Execute(ability);

		public void Exit(GameEntity agent, CommandEntity command) {
			var ability = _ability.GetEntityWithId(command.Ability.Value);
			ability.ReplaceEndCooldownTime((_clock.Time + ability.CooldownTime.Value).ToLong());
			ability.IsCooldown = true;

			// var animator = agent.Animator.Value;
			// animator.Play("Walking");
			// animator.SetLayerWeight(2, 0);
		}

		public bool Execute(GameEntity agent, CommandEntity command) {
			var animator = agent.Animator.Value;
			var weight = animator.GetLayerWeight(2);
			if (weight < 1)
				animator.SetLayerWeight(2, Mathf.Lerp(weight, 1, 0.1f));

			return !command.HasPause || command.Pause.Complete;
		}

		private void OnAnimation(GameEntity agent, CommandEntity command, string animationId) {
			if (!_animations.Has(animationId))
				return;

			var character = _character.GetEntityWithId(agent.Id.Value);
			var entry = _animations.Get(animationId);
			var duration = entry.Time;
			command.AddStartTime(Time.realtimeSinceStartup);
			command.AddPause(Time.realtimeSinceStartup, duration);
			var events = ListPool<AnimationEvent>.Get();
			events.AddRange(entry.Events);
			command.AddAnimationEvents(events);
			command.AddSpeed(character.CastSpeed.Value);

			var animator = agent.Animator.Value;
			animator.Play(entry.AnimationName);
			animator.SetLayerWeight(2, 0);

			if (command.IsStandingCommand && agent.HasDestination)
				agent.StopMovement();
		}
	}
}