using UnityEngine;
using Utopia;

namespace Ecs.Command {
	[InstallerGenerator(InstallerId.Game)]
	public class ExitAbilityCommandState : ICommandState {
		public EState State => EState.ExitAbility;

		public void Enter(GameEntity agent, CommandEntity command) {
			command.AddPause(Time.realtimeSinceStartup, .3f);
		}

		public void Exit(GameEntity agent, CommandEntity command) {
			var animator = agent.Animator.Value;
			animator.Play("Walking");
			animator.SetLayerWeight(2, 0);
		}

		public bool Execute(GameEntity agent, CommandEntity command) {
			var animator = agent.Animator.Value;
			var weight = animator.GetLayerWeight(2);
			if (weight > 0)
				animator.SetLayerWeight(2, Mathf.Lerp(weight, 0, 0.05f));

			return !command.HasPause || command.Pause.Complete;
		}
	}
}