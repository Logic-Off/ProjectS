using System;
using System.Collections.Generic;
using Ecs.Character;
using UnityEngine;

namespace Ecs.Item {
	[Serializable]
	public struct ClothData : IEquipment {
		public ItemId Id;
		
		[SerializeField] private List<EffectEntry> _effects;
		public List<EffectEntry> Effects => _effects;
	}
}