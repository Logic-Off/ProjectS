using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.Character {
	[InstallerGenerator(InstallerId.Game)]
	public sealed class UpdateMovementSpeedSystem : ReactiveSystem<CharacterEntity> {
		private readonly GameContext _game;

		public UpdateMovementSpeedSystem(CharacterContext character, GameContext game) : base(character) {
			_game = game;
		}

		protected override ICollector<CharacterEntity> GetTrigger(IContext<CharacterEntity> context)
			=> context.CreateCollector(CharacterMatcher.MovementSpeed.AddedOrRemoved());

		protected override bool Filter(CharacterEntity entity)
			=> entity.HasMovementSpeed;

		protected override void Execute(List<CharacterEntity> entities) {
			foreach (var entity in entities) {
				var gameEntity = _game.GetEntityWithId(entity.Id.Value);
				var speed = entity.MovementSpeed.Value;
				if (gameEntity.IsPlayer && gameEntity.HasNavmeshAgent)
					gameEntity.NavmeshAgent.Value.speed = speed;
				else if (gameEntity.IsNpc && gameEntity.HasAuthoringAgent) {
					var authoringAgent = gameEntity.AuthoringAgent.Value;
					var locomotion = authoringAgent.EntityLocomotion;
					locomotion.Speed = speed;
					authoringAgent.EntityLocomotion = locomotion;
				}
			}
		}
	}
}