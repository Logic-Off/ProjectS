using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ObjectFinderEditor.Scripts.Databases {
	/// <summary>
	/// БД для редактора, необходима для сохранения данных которые ищем
	/// </summary>
	// [CreateAssetMenu(menuName = "Editor/ObjectFinder", fileName = "ObjectFinder")]
	public class ObjectFinderScriptableObject : ScriptableObject {
		public GuidEntry GuidTarget;
		public List<Object> Targets = new List<Object>();
		public List<ObjectFinderFoundObjectEntry> FoundObjects = new List<ObjectFinderFoundObjectEntry>();



		[Serializable]
		public struct GuidEntry {
			public string Value;
		}
	}

	[Serializable]
	public class ObjectFinderFoundObjectEntry {
		public string TargetName;
		public Object Target;
		public List<Object> Values;

		public ObjectFinderFoundObjectEntry(Object target, List<Object> values) {
			var builder = new StringBuilder(target.name);
			builder.Append("__");
			builder.Append("[");
			if (values.Count == 0) {
				builder.Append("ОБЪЕКТ НЕ ИСПОЛЬЗУЕТСЯ!!!");
			} else {
				var separatorMaxCount = values.Count - 1;
				for (var i = 0; i < values.Count; i++) {
					var value = values[i];
					builder.Append(value.name);
					if (i < separatorMaxCount)
						builder.Append(", ");
				}
			}

			builder.Append("]");

			TargetName = builder.ToString();
			Target = target;
			Values = values;
		}
	}
}