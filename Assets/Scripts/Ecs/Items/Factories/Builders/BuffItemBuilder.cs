using System.Collections.Generic;
using Ecs.Character;
using Utopia;

namespace Ecs.Item.Builders {
	/// <summary>
	/// Билдер бафов итема
	/// </summary>
	[InstallerGenerator(InstallerId.Game)]
	public class BuffItemBuilder : IItemBuilder {
		private readonly IWeaponsDatabase _weaponsDatabase;
		private readonly IClothesDatabase _clothes;

		public BuffItemBuilder(IClothesDatabase clothes, IWeaponsDatabase weaponsDatabase) {
			_clothes = clothes;
			_weaponsDatabase = weaponsDatabase;
		}

		public void Build(ItemEntity item) {
			var entry = GetEquipment(item);
			if (entry == null)
				return;
			
			var buffs = item.Buffs.Values;
			var effects = new List<Effect>();

			foreach (var effectEntry in entry.Effects)
				effects.Add(new Effect(effectEntry.Stat, new Parameter(effectEntry.Count, effectEntry.Multiplier)));

			buffs.Add(new CharacterBuff(new BuffId(item.ItemId.Value), EBuffType.Trigger, effects, new List<TriggerEffect>()));

			item.ReplaceBuffs(buffs);
		}

		private IEquipment GetEquipment(ItemEntity item) {
			if (item.ItemType.Value is EItemType.Weapon)
				return _weaponsDatabase.Get(item.ItemId.Value);
			if (item.ItemType.Value is EItemType.Shield)
				return _clothes.Get(item.ItemId.Value);
			return null;
		}
	}
}