using System;
using Unity.Plastic.Newtonsoft.Json;

namespace LogicOff.DatabaseDownloader {
	public sealed class IdConverter<T> : JsonConverter {
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
			if (reader.TokenType == JsonToken.String)
				return Activator.CreateInstance(typeof(T), reader.Value);
			reader.Read(); // StartObject
			reader.Read(); // Property
			var value = reader.Value;
			reader.Read(); // EndObject
			return Activator.CreateInstance(typeof(T), value);
		}

		public override bool CanConvert(Type objectType) => _type == objectType;
	}
}