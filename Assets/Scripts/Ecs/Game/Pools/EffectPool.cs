using System;
using System.Collections.Generic;
using Common;
using Ecs.Common;
using UnityEngine;
using Utopia;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public class EffectPool : IDisposable {
		private readonly Dictionary<string, Queue<GameEntity>> _pools = new();
		private readonly GameContext _game;
		private readonly IClock _clock;

		public EffectPool(GameContext game, IClock clock) {
			_game = game;
			_clock = clock;
		}

		public GameEntity Get(string name, Id parent, Vector3 position, Quaternion rotation, float time) {
			var effect = Get(name, parent, position, rotation);
			effect.ReplaceEndTime(_clock.Time + time);
			return effect;
		}

		public GameEntity Get(string name, Id parent, Vector3 position, Quaternion rotation) {
			if (!_pools.ContainsKey(name))
				_pools.Add(name, new Queue<GameEntity>());

			if (_pools[name].Count == 0)
				return OnCreate(name, parent, position, rotation);

			var effect = _pools[name].Dequeue();
			effect.ReplaceParent(parent);
			effect.ReplaceNewPosition(position);
			effect.ReplaceNewRotation(rotation);
			effect.IsVisible = true;
			return effect;
		}

		private GameEntity OnCreate(string name, Id parent, Vector3 position, Quaternion rotation) {
			var effect = _game.CreateEntity();
			effect.AddId(IdGenerator.GetNext());
			effect.AddParent(parent);
			effect.AddPrefab(name);
			effect.AddPosition(position);
			effect.AddRotation(rotation);
			effect.IsVisible = true;

			effect.AddGameType(EGameType.Effect);
			return effect;
		}

		public void Release(GameEntity effect) {
			effect.IsVisible = false;
			effect.IsReturnedToPool = false;
			effect.RemoveParent();
			effect.RemoveEndTime();
			_pools[effect.Prefab.Value].Enqueue(effect);
		}

		public void Dispose() {
			foreach (var (name, handle) in _pools)
				handle.Clear();
			_pools.Clear();
		}
	}
}