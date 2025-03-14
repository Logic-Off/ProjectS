using Utopia;

namespace Ecs.Item {
	public interface IWeaponsDatabase : IDatabase<ItemId, WeaponEntry> { }

	[InstallerGenerator(InstallerId.Game)]
	public sealed class WeaponsDatabase : ADatabase<ItemId, WeaponEntry>, IWeaponsDatabase {
		public WeaponsDatabase(ItemsDatabaseAsset database) {
			foreach (var entry in database.Weapons)
				Add(entry.Id, entry);
		}
	}
}