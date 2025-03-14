using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace Common {
	public static class SerializedPropertyExtensions {
		public static T GetSerializedValue<T>(this SerializedProperty property) {
			object target = property.serializedObject.targetObject;
			var propertyNames = property.propertyPath.Split('.');
			var propertyNamesClean = new List<String>();

			for (var i = 0; i < propertyNames.Count(); i++) {
				if (propertyNames[i] == "Array") {
					if (i != (propertyNames.Count() - 1) && propertyNames[i + 1].StartsWith("data")) {
						var pos = int.Parse(propertyNames[i + 1].Split('[', ']')[1]);
						propertyNamesClean.Add($"-GetArray_{pos}");
						i++;
					} else
						propertyNamesClean.Add(propertyNames[i]);
				} else
					propertyNamesClean.Add(propertyNames[i]);
			}

			// Get the last object of the property path.
			foreach (var path in propertyNamesClean) {
				if (path.StartsWith("-GetArray")) {
					var split = path.Split('_');
					var index = int.Parse(split[split.Count() - 1]);
					var list = (IList)target;
					target = list[index];
				} else {
					target = target.GetType().GetField(path, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)!.GetValue(target);
				}
			}

			return (T)target;
		}
	}
}