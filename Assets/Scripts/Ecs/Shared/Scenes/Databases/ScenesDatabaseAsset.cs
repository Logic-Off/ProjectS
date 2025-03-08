using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Ecs.Shared {
	[CreateAssetMenu(menuName = "Databases/ScenesDatabase", fileName = "ScenesDatabase")]
	public sealed class ScenesDatabaseAsset : ScriptableObject {
		public List<SceneData> Scenes;
	}
}