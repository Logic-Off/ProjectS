using System;
using System.Collections.Generic;
using Ecs.Character;
using Ecs.Item;
using Newtonsoft.Json;

namespace LogicOff.DatabaseDownloader {
	public sealed class WeaponsDownloader : ADownloader<WeaponData> {
		private readonly Dictionary<int[], TableEntry> _buffs = new() {
			{ new[] { 7 }, new TableEntry("Buff") },
			{ new[] { 8 }, new TableEntry("ApplyTo") },
		};

		public override string Name => "Weapons";

		protected override JsonConverter[] Converters => new JsonConverter[] { new IdConverter<BuffId>() };

		protected override Dictionary<int[], TableEntry> Schema => new() {
			{ new[] { 0 }, new TableEntry("Id") },
			{ new[] { 1 }, new TableEntry("Type") },
			{ new[] { 2 }, new TableEntry("BaseAbility") },
			{ new[] { 3 }, new TableEntry("DamageType") },
			{ new[] { 4 }, new TableEntry("AnimationState") },
			// { new[] { 7 }, new TableEntry("Buffs", _buffs, true, true) },
		};

		public WeaponsDownloader() { }
		public WeaponsDownloader(string key, SheetEntry entry, Action<WeaponData[]> callback) : base(key, entry, callback) { }
	}
}