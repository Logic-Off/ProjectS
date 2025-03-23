using System;
using System.Collections.Generic;
using Ecs.Character;
using Ecs.Game;
using Newtonsoft.Json;

namespace LogicOff.DatabaseDownloader {
	public sealed class CharactersDownloader : ADownloader<CharacterData> {
		private readonly Dictionary<int[], TableEntry> _hostileTeams = new() { { new[] { 2 }, new TableEntry("") }, };

		private readonly Dictionary<int[], TableEntry> _levelEntries = new() {
			{ new[] { 6 }, new TableEntry("Psyche") },
			{ new[] { 7 }, new TableEntry("PsycheRegeneration") },
			{ new[] { 8 }, new TableEntry("Attack") },
			{ new[] { 9 }, new TableEntry("AttackRange") },
			{ new[] { 10 }, new TableEntry("MovementSpeed") },
			{ new[] { 11 }, new TableEntry("Stealth") },
			{ new[] { 12 }, new TableEntry("Observation") },
			{ new[] { 13 }, new TableEntry("Quietness") },
			{ new[] { 14 }, new TableEntry("VisionRange") },
			{ new[] { 15 }, new TableEntry("VisionAngle") },
			{ new[] { 16 }, new TableEntry("HearingRange") },
			{ new[] { 17 }, new TableEntry("Loudness") },
			{ new[] { 18 }, new TableEntry("CheckShelterChance") },
		};

		public override string Name => "Characters";

		protected override JsonConverter[] Converters => new JsonConverter[] { new EnumConverter<ETeam>() };

		protected override Dictionary<int[], TableEntry> Schema => new() {
			{ new[] { 0 }, new TableEntry("Name") },
			{ new[] { 1 }, new TableEntry("Team") },
			{ new[] { 2 }, new TableEntry("HostileTeams", _hostileTeams, true) },
			{ new[] { 3 }, new TableEntry("AlertSensorBuff") },
			{ new[] { 4 }, new TableEntry("AlertMovementBuff") },
			{ new[] { 5 }, new TableEntry("Prefab") },
			{ new[] { 6 }, new TableEntry("Levels", _levelEntries, true, true) },
			{ new[] { 19 }, new TableEntry("AttackAbilities", new() { { new[] { 19 }, new TableEntry("") }, }, true) },
			{ new[] { 20 }, new TableEntry("AnimationTypes", new() { { new[] { 20 }, new TableEntry("") }, }, true) },
			{ new[] { 21 }, new TableEntry("Animations", new() { { new[] { 21 }, new TableEntry("") }, }, true) },
			{ new[] { 22 }, new TableEntry("Resists", new() { { new[] { 22 }, new TableEntry("") }, }, true) },
			{ new[] { 23 }, new TableEntry("TakeDamageSound") },
		};

		public CharactersDownloader() { }
		public CharactersDownloader(string key, SheetEntry entry, Action<CharacterData[]> callback) : base(key, entry, callback) { }
	}
}