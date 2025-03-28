using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ecs.Animations {
	[CreateAssetMenu(menuName = "Databases/AnimationsDatabase", fileName = "AnimationsDatabase")]
	public sealed class AnimationsDatabaseAsset : ScriptableObject {
		[TableList]
		public List<AnimationData> All = new();
	}
}