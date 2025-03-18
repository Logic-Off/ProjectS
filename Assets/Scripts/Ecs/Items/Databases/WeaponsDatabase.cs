using Utopia;

namespace Ecs.Item {
	public interface IWeaponsDatabase : IDatabase<ItemId, WeaponData> { }

	[InstallerGenerator(InstallerId.Game)]
	public sealed class WeaponsDatabase : ADatabase<ItemId, WeaponData>, IWeaponsDatabase {
		public WeaponsDatabase(ItemsDatabaseAsset database) {
			foreach (var entry in database.Weapons)
				Add(entry.Id, entry);
		}
	}
}