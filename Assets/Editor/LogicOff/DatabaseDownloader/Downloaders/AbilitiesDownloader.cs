using System;
using System.Collections.Generic;
using Ecs.Ability;

namespace LogicOff.DatabaseDownloader {
	public sealed class AbilitiesDownloader : ADownloader<AbilityData> {
		private readonly Dictionary<int[], TableEntry> _parameters = new() {
			{ new[] { 7 }, new TableEntry("Name") },
			{ new[] { 8 }, new TableEntry("Value") },
		};

		public override string Name => "Abilities";

		protected override Dictionary<int[], TableEntry> Schema => new() {
			{ new[] { 0 }, new TableEntry("Id") },
			{ new[] { 1 }, new TableEntry("Type") },
			{ new[] { 2 }, new TableEntry("Cooldown") },
			{ new[] { 3 }, new TableEntry("IsLookAtTarget") },
			{ new[] { 4 }, new TableEntry("IsStandingCast") },
			{ new[] { 5 }, new TableEntry("Animation") },
			{ new[] { 6 }, new TableEntry("AbilityState") },
			{ new[] { 7 }, new TableEntry("Parameters", _parameters, true, true) },
		};
		public AbilitiesDownloader() { }
		public AbilitiesDownloader(string key, SheetEntry entry, Action<AbilityData[]> callback) : base(key, entry, callback) { }
	}
}