using Utopia;

namespace Ecs.Item {
	public interface IClothesDatabase : IDatabase<ItemId, ClothEntry> { }

	[InstallerGenerator(InstallerId.Game)]
	public sealed class ClothesDatabase : ADatabase<ItemId, ClothEntry>, IClothesDatabase {
		public ClothesDatabase(ItemsDatabaseAsset database) {
			foreach (var entry in database.Clothes)
				Add(entry.Id, entry);
		}
	}
}