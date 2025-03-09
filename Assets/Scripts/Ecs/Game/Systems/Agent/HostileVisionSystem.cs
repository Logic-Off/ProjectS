using System.Collections.Generic;
using Common;
using Ecs.Common;
using Unity.Collections;
using UnityEngine;
using Utopia;
using Zentitas;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public class HostileVisionSystem : IUpdateSystem {
		private readonly IGroup<GameEntity> _group;
		private readonly CharacterContext _character;

		public HostileVisionSystem(GameContext game, CharacterContext character) {
			_character = character;
			_group = game.GetGroup(GameMatcher.AllOf(GameMatcher.Team, GameMatcher.Transform).NoneOf(GameMatcher.Dead));
		}

		public void Update() => OnUpdateTargets();

		private void OnUpdateTargets() {
			var list = ListPool<GameEntity>.Get();
			_group.GetEntities(list);

			foreach (var entity in list) {
				var targets = entity.HostileTargets.Values;
				targets.Clear();
				OnCollectTargets(list, entity, targets);

				entity.ReplaceHostileTargets(targets);
			}

			list.ReturnToPool();
		}

		private void OnCollectTargets(List<GameEntity> list, GameEntity agent, List<TargetData> targets) {
			foreach (var other in list) {
				if (agent == other || !agent.HostileTo(other) || CheckTargets(targets, other.Id.Value))
					continue;

				var character = _character.GetEntityWithId(agent.Id.Value);
				if (CheckAngle(agent, other, character, out var positionWithOffset, out var direction))
					continue;

				var visionRange = GetVisionRange(agent, other, character);
				OnRaycast(agent, direction, visionRange, other, targets);
			}
		}

		private void OnRaycast(GameEntity agent, Vector3 direction, float visionRange, GameEntity target, List<TargetData> targets) {
			var results = new NativeArray<RaycastHit>(10, Allocator.TempJob);
			var commands = new NativeArray<RaycastCommand>(1, Allocator.TempJob);
			var position = agent.Position.Value + Vector3.up * 0.5f;

			var mask = target.LayerMask.Value;
			commands[0] = new RaycastCommand(position, direction, new QueryParameters(mask), visionRange);

			var handle = RaycastCommand.ScheduleBatch(commands, results, 1, 10);
			handle.Complete();

			var hitList = ListPool<RaycastHit>.Get();
			foreach (var hit in results) {
				if (hit.colliderInstanceID == 0 || hit.colliderInstanceID == agent.InstanceId.Value)
					continue;

				hitList.Add(hit);
			}

			OnAddTarget(target, targets, hitList);

			results.Dispose();
			commands.Dispose();
			ListPool<RaycastHit>.Release(hitList);
		}

		private void OnAddTarget(GameEntity target, List<TargetData> targets, List<RaycastHit> hitList) {
			var targetDistance = 0f;
			var isTarget = false;
			var nearestHitDistance = 1000f;

			var maskIndex = target.LayerMask.MaskIndex;
			foreach (var hit in hitList) {
				var hitIsTarget = target.HasCollider ? target.InstanceId.Value == hit.colliderInstanceID : target.InstanceId.Value == hit.transform.gameObject.GetInstanceID();

				var layer = hit.collider.transform.gameObject.layer;
				// Если это не наша цель, но объект того же слоя, то пропускаем(игнор линий врагов)
				if (!hitIsTarget && layer == maskIndex)
					continue;
				var hitDistance = hit.distance;
				if (hitIsTarget)
					targetDistance = hitDistance;

				if (hitDistance >= nearestHitDistance)
					continue;

				nearestHitDistance = hitDistance;
				isTarget = hitIsTarget;
			}

			if (isTarget)
				targets.Add(new TargetData(target.Id.Value, targetDistance, target.Position.Value));
		}

		private bool CheckTargets(List<TargetData> targets, Id id) {
			foreach (var target in targets)
				if (target.Id == id)
					return true;
			return false;
		}

		private float GetVisionRange(GameEntity entity, GameEntity other, CharacterEntity mainCharacter) {
			var visionMultiplier = GetVisionMultiplier(entity, other);
			var visionRange = mainCharacter.VisionRange.Value * visionMultiplier;
			return visionRange;
		}

		private static bool CheckAngle(GameEntity agent, GameEntity target, CharacterEntity mainCharacter, out Vector3 positionWithOffset, out Vector3 direction) {
			positionWithOffset = agent.Position.Value + Vector3.up * 0.5f;
			var targetPosition = target.Position.Value + Vector3.up * 0.5f;
			direction = (targetPosition - positionWithOffset).normalized;
			return Vector3.Angle(agent.Transform.Value.forward, direction) > mainCharacter.VisionAngle.Value * 0.5f;
		}

		private float GetVisionMultiplier(GameEntity agent, GameEntity target) {
			var agentParameters = _character.GetEntityWithId(agent.Id.Value).Parameters.Value;
			var targetParameters = _character.GetEntityWithId(target.Id.Value).Parameters.Value;
			var observation = agentParameters.Observation.Value;
			var stealth = targetParameters.Stealth.Value;

			return stealth <= 0 ? 1f : (observation / stealth).Clamp01();
		}
	}
}