// using LogicOff;
//
// namespace Ecs.Item.Builders {
// 	/// <summary>
// 	/// Билдер инструментов
// 	/// Author: Andrey Abramkin
// 	/// </summary>
// 	[Install(InstallerId.Game,  50_000)]
// 	public class ToolBuilder : IItemBuilder {
// 		private readonly IToolsDatabase _tools;
//
// 		public ToolBuilder(IToolsDatabase tools) => _tools = tools;
//
// 		public void Build(ItemsEntity entity) {
// 			if (entity.ItemType.Value != EItemType.Tool)
// 				return;
//
// 			var entry = _tools.Get(entity.ItemId);
// 			entity.AddToolType(entry.Type);
// 			entity.AddDurability(entry.Durability);
// 			entity.AddMaxDurability(entry.Durability);
// 		}
// 	}
// }