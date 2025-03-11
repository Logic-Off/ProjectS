using System.Collections.Generic;
using UnityEngine;

namespace Common {
	public static class CommonExtensions {
		public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);
		public static bool IsNotNullOrEmpty(this string value) => !string.IsNullOrEmpty(value);
		public static float Distance(this Vector3 self, Vector3 target) => (self - target).magnitude;
		public static int ConvertToLayerIndex(this LayerMask mask) => Mathf.RoundToInt(Mathf.Log(mask.value, 2));
	}
}