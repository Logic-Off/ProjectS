using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LogicOff.FavoriteObjects.Entries {
	/// <summary>
	///   Author: Andrey Abramkin
	/// </summary>
	[Serializable]
	public class FavoriteObjectItemEntry {
		public string Name;
		public Texture2D Icon;
		public Color IconBackgroundColor;
		public Object Object;
	}
}