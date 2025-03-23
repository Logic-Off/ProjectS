using System;
using System.Collections.Generic;
using Ecs.Item;

namespace LogicOff.DatabaseDownloader {
	public sealed class ContainerSettingsDownloader : ADownloader<ContainerSettingsData> {
		private readonly Dictionary<int[], TableEntry> _cells = new() {
			{new[] {1}, new TableEntry("Type")},
			{new[] {2}, new TableEntry("Quantity")}
		};

		public override string Name => "ContainerSettings";

		public ContainerSettingsDownloader() { }
		public ContainerSettingsDownloader(string key, SheetEntry entry, Action<ContainerSettingsData[]> callback) : base(key, entry, callback) { }

		protected override Dictionary<int[], TableEntry> Schema => new() {
			{new[] {0}, new TableEntry("Id")},
			{new[] {1}, new TableEntry("Cells", _cells, true, true)},
		};
	}
}