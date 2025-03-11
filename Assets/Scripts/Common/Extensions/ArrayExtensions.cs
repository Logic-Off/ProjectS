using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Common {
	public static class ArrayExtensions {
		[CanBeNull]
		public static T FirstOrDefault<T>(this IEnumerable<T> self, T defaultValue = default) {
			foreach (var element in self)
				return element;
			return defaultValue;
		}

		[CanBeNull]
		public static T FirstOrDefault<T>(this IEnumerable<T> self, Func<T, bool> func, T defaultValue = default) {
			foreach (var element in self)
				if (func(element))
					return element;

			return defaultValue;
		}

		public static T Find<T>(this IEnumerable<T> self, Func<T, bool> func) {
			foreach (var element in self)
				if (func(element))
					return element;

			throw new Exception("Value not found");
		}

		public static void RandomSort<T>(this IList<T> list) {
			var count = list.Count;
			for (var i = 0; i < count; i++) {
				var j = UnityEngine.Random.Range(0, count);
				(list[i], list[j]) = (list[j], list[i]); // свап элементов
			}
		}

		public static bool Any<T>(this IEnumerable<T> self, Func<T, bool> func) {
			if (self == null)
				throw new NullReferenceException(nameof(self));

			if (func == null)
				throw new NullReferenceException(nameof(func));

			foreach (var item in self)
				if (func(item))
					return true;

			return false;
		}

		public static T First<T>(this IEnumerable<T> self) {
			foreach (var element in self)
				return element;

			throw new Exception("Array is empty");
		}

		public static T First<T>(this T[] self) {
			foreach (var element in self)
				return element;

			throw new Exception("Array is empty");
		}

		public static T Last<T>(this IList<T> self) => self[self.Count - 1];

		public static T Random<T>(this IList<T> self) {
			var index = UnityEngine.Random.Range(0, self.Count);
			return self[index];
		}

		public static T Random<T>(this T[] self) {
			if (self.Length == 1)
				return self[0];
			var index = UnityEngine.Random.Range(0, self.Length);
			return self[index];
		}

		public static void Remove<T>(this List<T> self, ref List<T> removeItems) {
			foreach (var item in removeItems)
				self.Remove(item);
			removeItems.Clear();
		}
	}
}