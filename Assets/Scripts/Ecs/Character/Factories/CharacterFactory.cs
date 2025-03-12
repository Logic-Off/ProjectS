using System.Collections.Generic;
using Ecs.Ability;
using Ecs.AI;
using Ecs.Game;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utopia;

namespace Ecs.Character {
	[InstallerGenerator(InstallerId.Game)]
	public class CharacterFactory {
		private readonly CharacterContext _character;
		private readonly ICharacterDatabase _characterDatabase;
		private readonly ITeamLayerMaskDatabase _layerMaskDatabase;

		public CharacterFactory(CharacterContext character, ICharacterDatabase characterDatabase, ITeamLayerMaskDatabase layerMaskDatabase) {
			_character = character;
			_characterDatabase = characterDatabase;
			_layerMaskDatabase = layerMaskDatabase;
		}

		public CharacterEntity Create(GameEntity agent, string id, int level) {
			var data = _characterDatabase.Get(id);
			var parameters = data.Levels[level - 1].ToParameters; // Может стоит уровень добавить конечно, но считаем что всегда упорядоченно
			var entity = _character.CreateEntity();

			entity.AddId(agent.Id.Value);
			entity.AddParameters(parameters);
			entity.AddHealth(parameters.Health.Value);
			entity.AddMaxHealth(parameters.Health.Value);
			entity.AddHealthModifier(new List<StatModifier>());
			entity.AddAttack(parameters.Attack.Value);
			entity.AddCastSpeed(parameters.CastSpeed.Value);
			entity.AddMovementSpeed(parameters.MovementSpeed.Value);
			entity.AddVisionRange(parameters.VisionRange.Value);
			entity.AddVisionAngle(parameters.VisionAngle.Value);
			entity.AddStealth(parameters.Stealth.Value);
			entity.AddObservation(parameters.Observation.Value);
			entity.IsPlayer = agent.IsPlayer;
			entity.IsNpc = agent.IsNpc;

			agent.IsFsm = true;

			agent.AddTeam(data.Team);
			agent.AddHostileTeams(data.HostileTeams);

			agent.AddCurrentAnimationState(data.BaseAnimationState);
			agent.AddPreviousAnimationState(data.BaseAnimationState);

			agent.AddHostileTargets(new List<TargetData>());
			agent.AddAttackTargets(new List<TargetData>());

			agent.AddAbilities(new List<AbilityId>(data.Abilities));

			var layerMaskData = _layerMaskDatabase.Get(agent.Team.Value);
			agent.AddLayerMask(layerMaskData.Mask, layerMaskData.MaskIndex);

			agent.AddChangeItems(new Dictionary<EItemPosition, string>());
			agent.AddCurrentItems(new Dictionary<EItemPosition, AsyncOperationHandle<GameObject>>());

			// Вынести в биндер
			if (agent.IsNpc)
				agent.AddPreviousAiAction(EAiAction.Idle);

			return entity;
		}
	}
}