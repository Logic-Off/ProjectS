using System;
using System.Collections.Generic;
using Ecs.Ability;
using Ecs.Character;
using UnityEngine;

namespace Ecs.Item {
	[Serializable]
	public struct WeaponEntry : IEquipment {
		public ItemId Id;
		public AbilityId BaseAbility;

		[SerializeField] private List<EffectEntry> _effects;
		public List<EffectEntry> Effects => _effects;
	}
}