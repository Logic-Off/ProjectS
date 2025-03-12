using System;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Common {
	public static class CommonExtensions {
		private static Random _random;

		static CommonExtensions() {
			_random = new Random((uint) (DateTime.Now - DateTime.UnixEpoch).TotalMilliseconds);
			_random.NextFloat(); // https://discussions.unity.com/t/unity-mathematics-random-always-returning-zero/771118/10
		}

		public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);
		public static bool IsNotNullOrEmpty(this string value) => !string.IsNullOrEmpty(value);
		public static float Distance(this Vector3 self, Vector3 target) => (self - target).magnitude;
		public static int ConvertToLayerIndex(this LayerMask mask) => Mathf.RoundToInt(Mathf.Log(mask.value, 2));

		public static Vector2 RandomPositionInRadius(this Vector2 center, float radius) {
			var angle = _random.NextFloat(0, math.PI * 2);
			var distance = _random.NextFloat(0, radius);
			var x = center.x + distance * math.cos(angle);
			var y = center.y + distance * math.sin(angle);
			return new Vector2(x, y);
		}

		public static Vector2 RandomPositionAtCircle(this Vector2 center, float radius) {
			var angle = _random.NextFloat(0, math.PI * 2);
			var x = center.x + radius * math.cos(angle);
			var y = center.y + radius * math.sin(angle);
			return new Vector2(x, y);
		}

		public static Vector3 RandomPositionInRadius(this Vector3 center, float radius) {
			var angle = _random.NextFloat(0, math.PI * 2);
			var distance = _random.NextFloat(0, radius);
			var x = center.x + distance * math.cos(angle);
			var z = center.z + distance * math.sin(angle);
			return new Vector3(x, center.y, z);
		}

		public static Vector3 RandomPositionAtCircle(this Vector3 center, float radius) {
			var angle = _random.NextFloat(0, math.PI * 2);
			var x = center.x + radius * math.cos(angle);
			var z = center.z + radius * math.sin(angle);
			return new Vector3(x, center.y, z);
		}
	}
}