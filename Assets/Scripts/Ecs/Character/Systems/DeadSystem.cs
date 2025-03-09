using System.Collections.Generic;
using Utopia;
using Zentitas;

namespace Ecs.Character {
	[InstallerGenerator(InstallerId.Game)]
	public class DeadSystem : ReactiveSystem<CharacterEntity> {
		private readonly GameContext _game;

		public DeadSystem(CharacterContext character, GameContext game) : base(character) => _game = game;

		protected override ICollector<CharacterEntity> GetTrigger(IContext<CharacterEntity> context)
			=> context.CreateCollector(CharacterMatcher.Health);

		protected override bool Filter(CharacterEntity entity)
			=> entity.HasHealth;

		protected override void Execute(List<CharacterEntity> entities) {
			foreach (var entity in entities) {
				var isDead = entity.Health.Value <= 0;
				entity.IsDead = isDead;
				var agent = _game.GetEntityWithId(entity.Id.Value);
				agent.IsDead = isDead;
			}
		}
	}
}