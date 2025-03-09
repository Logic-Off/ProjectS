using Utopia;

namespace Ecs.AI {
	[InstallerGenerator(InstallerId.Game)]
	public class FleeAction : IAction {
		private const float FleeRange = 5f;
		private const float HealthThreshold = 0.3f;

		private readonly CharacterContext _character;

		public FleeAction(CharacterContext character) {
			_character = character;
		}

		public float GetScore(GameEntity entity) {
			var character = _character.GetEntityWithId(entity.Id.Value);
			if (character == null || !entity.HasPosition || !entity.HasHostileTarget)
				return 0f;

			var health = character.Health.Value / character.MaxHealth.Value;
			var distanceToPlayer = entity.HostileTarget.Value.Distance;
			var shouldFlee = health < HealthThreshold && distanceToPlayer < FleeRange;
			return shouldFlee ? 90f : 0f;
		}

		public void Execute(GameEntity agent) {
			D.Error("[AttackAction]", agent.Id.Value, " убегает");
		}
	}
}