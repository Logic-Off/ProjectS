using Utopia;

namespace Ecs.Item {
	public interface IClothesDatabase : IDatabase<ItemId, ClothData> { }

	[InstallerGenerator(InstallerId.Game)]
	public sealed class ClothesDatabase : ADatabase<ItemId, ClothData>, IClothesDatabase {
		public ClothesDatabase(ItemsDatabaseAsset database) {
			foreach (var entry in database.Clothes)
				Add(entry.Id, entry);
		}
	}
}