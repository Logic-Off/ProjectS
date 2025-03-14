using System.Collections.Generic;
using UnityEngine.Pool;
using Utopia;

namespace Ecs.Inventory {
	[InstallerGenerator(InstallerId.Game)]
	public class GameGiveItemsProcessor : AGiveItemsProcessor {
		private readonly IPlayerInventory _playerInventory;
		public GameGiveItemsProcessor(IPlayerInventory playerInventory) => _playerInventory = playerInventory;

		public override bool OnGive(List<IItemData> items) {
			var removed = ListPool<IItemData>.Get();
			foreach (var itemRecord in items) {
				if (!_playerInventory.IsEnoughSpace(itemRecord.Id, itemRecord.Quantity))
					break;

				_playerInventory.Add(itemRecord.Id, itemRecord.Quantity);
				removed.Add(itemRecord);
			}

			foreach (var itemRecord in removed)
				items.Remove(itemRecord);

			ListPool<IItemData>.Release(removed);
			return items.Count == 0;
		}
	}
}