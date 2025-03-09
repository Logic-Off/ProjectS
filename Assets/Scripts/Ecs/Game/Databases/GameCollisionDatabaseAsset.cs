using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Ecs.Game {
	[CreateAssetMenu(menuName = "Databases/GameCollisionDatabase", fileName = "GameCollisionDatabase")]
	public sealed class GameCollisionDatabaseAsset : ScriptableObject {
		public List<TeamLayerMaskData> TeamLayerMaskDataList;
		public List<ProjectileCollisionMaskData> ProjectileCollisionMaskList;

#if UNITY_EDITOR
		[ContextMenu("Set index")]
		private void SetIndex() {
			for (var index = 0; index < TeamLayerMaskDataList.Count; index++) {
				var data = TeamLayerMaskDataList[index];
				data.MaskIndex = data.Mask.ConvertToLayerIndex();
				TeamLayerMaskDataList[index] = data;
			}
		}
#endif
	}
}