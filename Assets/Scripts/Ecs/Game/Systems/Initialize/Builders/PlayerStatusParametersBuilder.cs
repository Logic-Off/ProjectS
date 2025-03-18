using Ecs.Character;
using Utopia;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public class PlayerStatusParametersBuilder : IAgentBuilder {
		private readonly CharacterContext _character;
		private readonly ICharacterDatabase _characterDatabase;

		public PlayerStatusParametersBuilder(CharacterContext character, ICharacterDatabase characterDatabase) {
			_character = character;
			_characterDatabase = characterDatabase;
		}

		public bool Accept(GameEntity command) => command.IsPlayer;

		public void Apply(GameEntity animationEvent) {
			var data = _characterDatabase.Get("Player");
			var parameters = data.Levels[0].ToParameters;
			var entity = _character.GetEntityWithId(animationEvent.Id.Value);

			entity.AddHunger(parameters.Hunger.Value);
			entity.AddMaxHunger(parameters.Hunger.Value);
			entity.AddThirst(parameters.Thirst.Value);
			entity.AddMaxThirst(parameters.Thirst.Value);
			entity.AddPsyche(parameters.Psyche.Value);
			entity.AddMaxPsyche(parameters.Psyche.Value);
			entity.AddCold(parameters.Cold.Value);
			entity.AddMaxCold(parameters.Cold.Value);
			entity.AddRadiation(parameters.Radiation.Value);
			entity.AddMaxRadiation(parameters.Radiation.Value);
		}
	}
}