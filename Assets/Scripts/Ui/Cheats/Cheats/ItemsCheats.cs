using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Ecs.Inventory;
using Ecs.Item;
using UnityEngine.Localization.Settings;
using Utopia;

namespace Ui.Cheats {
	[InstallerGenerator(InstallerId.Cheats)]
	public class ItemsCheats : ACheat {
		private int _quantity = 1;
		private ItemsComparer _comparer = new();
		public override string Name => "Items";
		public override int Order => 100;

		private readonly IItemsDatabase _items;
		private readonly IGiveItemsFacade _giveItems;

		public ItemsCheats(IItemsDatabase items, IGiveItemsFacade giveItems) {
			_items = items;
			_giveItems = giveItems;
		}

		public override void Create() {
			CreateQuantityButton(1);
			CreateQuantityButton(5);
			CreateQuantityButton(20);

			OnCreateItems();
		}

		private async void OnCreateItems() {
			var itemsByType = new Dictionary<EItemType, List<ItemData>>();
			var items = new List<ItemData>(_items.Values);
			items.Sort(_comparer);

			foreach (var entry in items) {
				if (!itemsByType.ContainsKey(entry.Type))
					itemsByType.Add(entry.Type, new List<ItemData>());

				itemsByType[entry.Type].Add(entry);
			}

			foreach (var pair in itemsByType) {
				AddHeader(pair.Key.ToString());
				foreach (var item in pair.Value) {
					var name = await GetLocalization($"{item.Id}.Name");
					AddButton($"{item.Id}{name}", () => _giveItems.OnGive(new List<IItemData> { new InventoryItemData(item.Id, _quantity) }));
				}
			}
		}

		private void CreateQuantityButton(int quantity) => AddButton($"QuantityButton{quantity}", $"Количество {quantity}", () => _quantity = quantity);

		private async UniTask<string> GetLocalization(string key) {
			var handle = LocalizationSettings.StringDatabase.GetTableAsync(LocalizationSettings.StringDatabase.DefaultTable);
			await handle.ToUniTask();

			if (handle.Result.SharedData.Contains(key)) {
				var localizedHandle = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(key);
				await localizedHandle;
				return $"\n({localizedHandle.Result})";
			}

			return string.Empty;
		}

		private sealed class ItemsComparer : IComparer<ItemData> {
			public int Compare(ItemData a, ItemData b)
				// ReSharper disable twice PossibleNullReferenceException
				=> a.Type.CompareTo(b.Type);
		}
	}
}