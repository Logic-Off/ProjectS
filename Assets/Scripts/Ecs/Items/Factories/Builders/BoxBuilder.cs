// using LogicOff;
//
// namespace Ecs.Item.Builders {
// 	/// <summary>
// 	/// Билдер боксов
// 	/// Author: Andrey Abramkin
// 	/// </summary>
// 	[Install(InstallerId.Game, 50_000)]
// 	public class BoxBuilder : IItemBuilder {
// 		private readonly IBoxDatabase _database;
//
// 		public BoxBuilder(IBoxDatabase database) => _database = database;
//
// 		public void Build(ItemsEntity entity) {
// 			if (entity.ItemType.Value != EItemType.Box)
// 				return;
//
// 			var entry = _database.Get(entity.ItemId);
// 			entity.AddProductId(entry.ProductId);
// 		}
// 	}
// }