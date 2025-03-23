using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LogicOff.DatabaseDownloader {
	public sealed class FlagEnumConverter<T> : JsonConverter where T : struct {
		private readonly Type _type = typeof(T);
		public override bool CanWrite => false;

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
			=> throw new NotImplementedException();

		public override object ReadJson(
			JsonReader reader,
			Type objectType,
			object existingValue,
			JsonSerializer serializer
		) {
			if (reader.TokenType != JsonToken.StartArray)
				throw new NotImplementedException(reader.TokenType.ToString());
			var token = JToken.Load(reader);
			var items = token.ToObject<List<string>>();
			var sum = 0;
			var t = typeof(T);
			foreach (var n in items)
				if (n != "")
					sum |= (int)Enum.Parse(t, n);
			return sum;
		}

		public override bool CanConvert(Type objectType) => _type == objectType;
	}
}