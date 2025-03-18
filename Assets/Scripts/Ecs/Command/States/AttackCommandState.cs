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
	public class AttackCommandState : ICommandState {
		public EState State => EState.Attack;

		private readonly IAnimationsDatabase _animations;
		private readonly IClock _clock;
		private readonly AbilityContext _ability;
		private readonly CharacterContext _character;
		private readonly AbilityStrategy _abilityStrategy;
		private readonly GameContext _game;

		public AttackCommandState(
			IAnimationsDatabase animations,
			IClock clock,
			AbilityContext ability,
			CharacterContext character,
			AbilityStrategy abilityStrategy,
			GameContext game
		) {
			_animations = animations;
			_clock = clock;
			_ability = ability;
			_character = character;
			_abilityStrategy = abilityStrategy;
			_game = game;
		}

		public void Enter(GameEntity agent, CommandEntity command) {
			var ability = _ability.GetEntityWithId(command.Ability.Value);
			var animator = agent.Animator.Value;
			animator.SetBool(ability.AbilityState.Value, true);
		}

		private void OnAbility(GameEntity agent, CommandEntity command, AbilityEntity ability) {
			var attackTarget = agent.AttackTarget.Value.Id;
			var target = _game.GetEntityWithId(attackTarget);
			if (!command.HasTarget || command.Target.Value != attackTarget)
				command.ReplaceTarget(attackTarget);
			_abilityStrategy.Execute(ability);
			ability.IsCooldown = true;
			agent.LookAt(target);
			var duration = OnAnimation(agent, command, ability.AnimationId.Value);

			ability.ReplaceEndCooldownTime((_clock.Time + ability.CooldownTime.Value + duration).ToLong());
		}

		public void Exit(GameEntity agent, CommandEntity command) {
			var ability = _ability.GetEntityWithId(command.Ability.Value);
			var animator = agent.Animator.Value;
			animator.SetBool(ability.AbilityState.Value, false);
		}

		public bool Execute(GameEntity agent, CommandEntity command) {
			if (!agent.IsAttackLoopedCast)
				return true;

			var animator = agent.Animator.Value;
			var weight = animator.GetLayerWeight(2);
			if (weight < 1)
				animator.SetLayerWeight(2, Mathf.Lerp(weight, 1, 0.1f));

			if (!agent.HasAttackTarget)
				return false;

			if (command.HasPause && !command.Pause.Complete)
				return false;

			var ability = _ability.GetEntityWithId(command.Ability.Value);
			if (ability.IsCooldown)
				return false;

			OnAbility(agent, command, ability);

			return false;
		}

		private float OnAnimation(GameEntity agent, CommandEntity command, string animationId) {
			if (!_animations.Has(animationId))
				return 0f;

			var character = _character.GetEntityWithId(agent.Id.Value);
			var entry = _animations.Get(animationId);
			var duration = entry.Time / character.CastSpeed.Value;
			command.ReplaceStartTime(Time.realtimeSinceStartup);
			command.ReplacePause(Time.realtimeSinceStartup, duration);
			var events = ListPool<AnimationEvent>.Get();
			foreach (var animationEvent in entry.Events) {
				var data = animationEvent;
				data.Time /= character.CastSpeed.Value;
				events.Add(data);
			}

			command.ReplaceAnimationEvents(events);
			command.ReplaceSpeed(character.CastSpeed.Value);

			var animator = agent.Animator.Value;
			animator.Play(entry.AnimationName);

			if (command.IsStandingCommand && agent.HasDestination)
				agent.StopMovement();
			return duration;
		}
	}
}