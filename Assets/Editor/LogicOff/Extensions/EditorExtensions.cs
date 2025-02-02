using UnityEditor;
using UnityEngine.UIElements;

namespace LogicOff.Extensions {
	/// <summary>
	///   Author: Andrey Abramkin
	/// </summary>
	public static class EditorExtensions {
		public static VisualTreeAsset LoadUxmlAsset(this string value)
			=> AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(value);
	}
}