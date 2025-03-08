using System.Collections.Generic;
using UnityEngine;

namespace Ecs.Character {

	[CreateAssetMenu(menuName = "Databases/CharacterDatabase", fileName = "CharacterDatabase")]
	public sealed class CharacterDatabaseAsset : ScriptableObject {
		public List<CharacterData> Characters;
	}
}