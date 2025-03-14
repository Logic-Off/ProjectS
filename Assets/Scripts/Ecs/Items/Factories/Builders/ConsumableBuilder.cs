// using Ecs.Character;
// using LogicOff;
//
// namespace Ecs.Item.Builders {
// 	[Install(InstallerId.Game, 50_000)]
// 	public class ConsumableBuilder : IItemBuilder {
// 		private readonly IConsumablesDatabase _consumablesDatabase;
// 		private readonly IBuffsDatabase _buffsDatabase;
//
// 		public ConsumableBuilder(IConsumablesDatabase consumablesDatabase, IBuffsDatabase buffsDatabase) {
// 			_consumablesDatabase = consumablesDatabase;
// 			_buffsDatabase = buffsDatabase;
// 		}
//
// 		public void Build(ItemsEntity entity) {
// 			if (entity.ItemType.Value != EItemType.Consumable || !_consumablesDatabase.Has(entity.ItemId.Value))
// 				return;
//
// 			var entry = _consumablesDatabase.Get(entity.ItemId);
// 			var buffs = entity.Buffs.Values;
// 			foreach (var buffId in entry.Buffs) {
// 				var buff = _buffsDatabase.Get(buffId);
// 				buffs.Add(new CharacterBuff(buff.Id, buff.GetBuffEffects(), buff.Type, buff.Duration));
// 			}
//
// 			entity.ReplaceBuffs(buffs);
// 		}
// 	}
// }