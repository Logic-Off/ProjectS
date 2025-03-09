using System.Collections.Generic;
using UnityEngine;

namespace Common {
	public static class CommonExtensions {
		public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);
		public static bool IsNotNullOrEmpty(this string value) => !string.IsNullOrEmpty(value);
		public static float Distance(this Vector3 self, Vector3 target) => (self - target).magnitude;
		public static int ConvertToLayerIndex(this LayerMask mask) => Mathf.RoundToInt(Mathf.Log(mask.value, 2));

		public static void Remove<T>(this List<T> self, ref List<T> removeItems) {
			foreach (var item in removeItems)
				self.Remove(item);
			removeItems.Clear();
		}
	}
}