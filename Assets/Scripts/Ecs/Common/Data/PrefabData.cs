using System;
using UnityEngine.AddressableAssets;

namespace Ecs.Common {
	[Serializable]
	public struct PrefabData {
		public string Name;
		public AssetReference AssetReference;
	}
}