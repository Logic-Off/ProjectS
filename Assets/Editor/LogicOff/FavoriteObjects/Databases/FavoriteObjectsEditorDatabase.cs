using LogicOff.FavoriteObjects.Entries;
using UnityEngine;

namespace LogicOff.FavoriteObjects.Databases {
	/// <summary>
	///   Author: Andrey Abramkin
	/// </summary>
	[CreateAssetMenu(menuName = "Editor/Databases/FavoriteObjectsDatabase", fileName = "FavoriteObjectsDatabase")]
	public sealed class FavoriteObjectsEditorDatabase : ScriptableObject {
		public FavoriteObjectItemEntry[] All;
	}
}