using System.Collections.Generic;
using UnityEngine;

namespace Ecs.Animations {
	[CreateAssetMenu(menuName = "Databases/AnimationsDatabase", fileName = "AnimationsDatabase")]
	public sealed class AnimationsDatabaseAsset : ScriptableObject {
		public List<AnimationData> All = new();
	}

}