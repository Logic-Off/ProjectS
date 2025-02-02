using System;
using UnityEngine.AddressableAssets;

namespace Ecs.Shared {
	[Serializable]
	public struct SceneData {
		public LocationId Id;
		public AssetReference Scene;
		public AssetReference Manager;
	}
}