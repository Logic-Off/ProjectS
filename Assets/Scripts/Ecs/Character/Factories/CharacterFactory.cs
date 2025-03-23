using System.Collections.Generic;
using Common;
using Ecs.Ability;
using Ecs.AI;
using Ecs.Common;
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
		private readonly List<IAgentBuilder> _builders;
		private readonly IAbilityFactory _abilityFactory;

		public CharacterFactory(
			CharacterContext character,
			ICharacterDatabase characterDatabase,
			ITeamLayerMaskDatabase layerMaskDatabase,
			List<IAgentBuilder> builders,
			IAbilityFactory abilityFactory
		) {
			_character = character;
			_characterDatabase = characterDatabase;
			_layerMaskDatabase = layerMaskDatabase;
			_builders = builders;
			_abilityFactory = abilityFactory;
		}

		public CharacterEntity Create(GameEntity agent, string id, int level) {
			var data = _characterDatabase.Get(id);
			var entity = _character.CreateEntity();

			var parameters = data.Levels[level - 1].ToParameters; // Может стоит уровень добавить конечно, но считаем что всегда упорядоченно

			entity.AddParameters(parameters);
			entity.AddHealth(parameters.Health.Value);
			entity.AddMaxHealth(parameters.Health.Value);
			entity.AddStatModifier(new List<StatModifier>());
			entity.AddAttack(parameters.Attack.Value);
			entity.AddCastSpeed(parameters.CastSpeed.Value);
			entity.AddMovementSpeed(parameters.MovementSpeed.Value);
			entity.AddVisionRange(parameters.VisionRange.Value);
			entity.AddVisionAngle(parameters.VisionAngle.Value);
			entity.AddStealth(parameters.Stealth.Value);
			entity.AddObservation(parameters.Observation.Value);
			entity.AddResistanceNormalDamage(parameters.ResistanceNormalDamage.Value);
			entity.AddResistancePenetratingDamage(parameters.ResistancePiercingDamage.Value);
			entity.AddResistanceCrushingDamage(parameters.ResistanceCrushingDamage.Value);
			entity.AddId(agent.Id.Value);
			entity.IsPlayer = agent.IsPlayer;
			entity.IsNpc = agent.IsNpc;

			entity.AddBuffs(new List<ABuff>());
			entity.AddBuffModifier(new List<BuffModifier>());

			agent.IsFsm = true;

			agent.AddTeam(data.Team);
			agent.AddHostileTeams(data.HostileTeams);
			agent.AddGameType(EGameType.Agent);

			agent.AddCurrentAnimationState(data.BaseAnimationState);
			agent.AddPreviousAnimationState(data.BaseAnimationState);

			agent.AddHostileTargets(new List<TargetData>());
			agent.AddAttackTargets(new List<TargetData>());

			var abilities = new List<Id>();
			if (data.BaseAbility.ToString().IsNotNullOrEmpty()) {
				var ability = _abilityFactory.Create(data.BaseAbility, entity.Id.Value);
				agent.AddBaseAbility(ability.Id.Value);
				agent.AddDefaultAbility(ability.Id.Value);
				abilities.Add(ability.Id.Value);
			}

			foreach (var abilityId in data.Abilities) {
				var ability = _abilityFactory.Create(abilityId, entity.Id.Value);
				abilities.Add(ability.Id.Value);
			}

			agent.AddAbilities(abilities);

			var layerMaskData = _layerMaskDatabase.Get(agent.Team.Value);
			agent.AddLayerMask(layerMaskData.Mask, layerMaskData.MaskIndex);

			agent.AddChangeItems(new Dictionary<EItemPosition, string>());
			agent.AddCurrentItems(new Dictionary<EItemPosition, AsyncOperationHandle<GameObject>>());

			// Вынести в биндер
			if (agent.IsNpc) {
				agent.AddPreviousAiAction(EAiAction.Idle);
				agent.AddActions(data.Actions);
			}

			OnBuild(agent);

			return entity;
		}

		public void OnBuild(GameEntity agent) {
			foreach (var builder in _builders)
				if (builder.Accept(agent))
					builder.Apply(agent);
		}
	}
}