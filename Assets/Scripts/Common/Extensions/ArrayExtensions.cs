using System;
using System.Collections.Generic;

namespace Common {
	public static class ArrayExtensions {
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
	}
}