using System;
using System.Collections.Generic;
using Ecs.Character;

namespace LogicOff.DatabaseDownloader {
	public sealed class BuffsDownloader : ADownloader<BuffData> {
		private readonly Dictionary<int[], TableEntry> _effects = new() {
			{ new[] { 1 }, new TableEntry("Stat") },
			{ new[] { 2 }, new TableEntry("Count") },
			{ new[] { 3 }, new TableEntry("Multiplier") },
		};

		public override string Name => "Buffs";

		protected override Dictionary<int[], TableEntry> Schema => new() {
			{ new[] { 0 }, new TableEntry("Id") },
			{ new[] { 1 }, new TableEntry("Effects", _effects, true, true) },
			{ new[] { 4 }, new TableEntry("Type") },
			{ new[] { 5 }, new TableEntry("Duration") },
			{ new[] { 6 }, new TableEntry("MaxStack") },
		};

		public BuffsDownloader() { }
		public BuffsDownloader(string key, SheetEntry entry, Action<BuffData[]> callback) : base(key, entry, callback) { }
	}
}