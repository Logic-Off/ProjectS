using System;
using System.Collections.Generic;
using Ecs.Item;
using Newtonsoft.Json;

namespace LogicOff.DatabaseDownloader {
	public sealed class ItemsDownloader : ADownloader<ItemData> {
		public override string Name => "Items";

		protected override JsonConverter[] Converters => new JsonConverter[] { };

		protected override Dictionary<int[], TableEntry> Schema => new() {
			{new[] {0}, new TableEntry("Id")},
			{new[] {1}, new TableEntry("Type")},
			{new[] {2}, new TableEntry("StackSize")},
		};

		public ItemsDownloader() { }
		public ItemsDownloader(string key, SheetEntry entry, Action<ItemData[]> callback) : base(key, entry, callback) { }
	}
}