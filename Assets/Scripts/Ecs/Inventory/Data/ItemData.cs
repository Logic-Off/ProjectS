using System;
using Ecs.Item;
using Newtonsoft.Json;
using UnityEngine;

namespace Ecs.Inventory {
	[Serializable]
	[JsonObject(MemberSerialization.Fields)]
	public struct ItemData : IItemData {
		public ItemId Id => _id;
		public int Quantity => _quantity;
		[SerializeField] [JsonProperty] private ItemId _id;
		[SerializeField] [JsonProperty] private int _quantity;

		public ItemData(ItemId id, int quantity) {
			_id = id;
			_quantity = quantity;
		}
	}
}