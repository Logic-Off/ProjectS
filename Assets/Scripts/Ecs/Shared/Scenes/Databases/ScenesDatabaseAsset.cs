using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Ecs.Shared {
	[CreateAssetMenu(menuName = "Databases/ScenesDatabase", fileName = "ScenesDatabase")]
	public sealed class ScenesDatabaseAsset : ScriptableObjectInstaller {
		public List<SceneData> Scenes;
		public override void InstallBindings() => Container.BindInstance(this);
	}
}