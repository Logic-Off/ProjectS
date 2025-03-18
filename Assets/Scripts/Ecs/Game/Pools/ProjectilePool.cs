using System;
using System.Collections.Generic;
using Ecs.Common;
using UnityEngine;
using Utopia;

namespace Ecs.Game {
	[InstallerGenerator(InstallerId.Game)]
	public class ProjectilePool : IDisposable {
		private readonly Dictionary<string, Queue<GameEntity>> _pools = new();
		private readonly GameContext _game;

		public ProjectilePool(GameContext game) => _game = game;

		public GameEntity Get(string name, Id owner, Vector3 position, Quaternion rotation) {
			if (!_pools.ContainsKey(name))
				_pools.Add(name, new Queue<GameEntity>());

			if (_pools[name].Count == 0)
				return OnCreate(name, owner, position, rotation);

			var projectile = _pools[name].Dequeue();
			projectile.ReplaceOwner(owner);
			projectile.ReplaceNewPosition(position);
			projectile.ReplaceNewRotation(rotation);
			projectile.ReplaceRigidbodyForce(10, ForceMode.Impulse);
			projectile.IsVisible = true;
			return projectile;
		}

		private GameEntity OnCreate(string name, Id owner, Vector3 position, Quaternion rotation) {
			var projectile = _game.CreateEntity();
			projectile.AddId(IdGenerator.GetNext());
			projectile.AddOwner(owner);
			projectile.AddPrefab(name);
			projectile.AddPosition(position);
			projectile.AddRotation(rotation);
			projectile.AddRigidbodyForce(10, ForceMode.Impulse);
			projectile.IsVisible = true;

			projectile.AddGameType(EGameType.Projectile);
			return projectile;
		}

		public void Release(GameEntity projectile) {
			projectile.IsVisible = false;
			projectile.IsReturnedToPool = false;
			_pools[projectile.Prefab.Value].Enqueue(projectile);
		}

		public void Dispose() {
			foreach (var (name, handle) in _pools)
				handle.Clear();
			_pools.Clear();
		}
	}
}