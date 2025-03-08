using Utopia;

namespace Ecs.Character {
	[InstallerGenerator("Game")]
	public class CharacterFactory {
		private readonly CharacterContext _character;
		private readonly ICharacterDatabase _characterDatabase;

		public CharacterFactory(CharacterContext character, ICharacterDatabase characterDatabase) {
			_character = character;
			_characterDatabase = characterDatabase;
		}

		public CharacterEntity Create(GameEntity agent, string id, int level) {
			var data = _characterDatabase.Get(id);
			var parameters = data.Levels[level - 1].ToParameters; // Может стоит уровень добавить конечно, но считаем что всегда упорядоченно
			var entity = _character.CreateEntity();
			
			entity.AddId(agent.Id.Value);
			entity.AddParameters(parameters);
			entity.AddHealth(parameters.Health.Value);
			entity.AddMaxHealth(parameters.Health.Value);
			entity.AddAttack(parameters.Attack.Value);
			entity.AddCastSpeed(parameters.CastSpeed.Value);
			entity.AddMovementSpeed(parameters.MovementSpeed.Value);
			entity.IsPlayer = agent.IsPlayer;
			entity.IsNpc = agent.IsNpc;

			agent.AddTeam(data.Team);
			agent.AddHostileTeams(data.HostileTeams);
			agent.AddCurrentAnimationState(data.BaseAnimationState);
			
			return entity;
		}
	}
}