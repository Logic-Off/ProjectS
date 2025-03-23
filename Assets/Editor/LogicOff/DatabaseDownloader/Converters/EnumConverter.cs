using System;
using Newtonsoft.Json;

namespace LogicOff.DatabaseDownloader {
	public sealed class EnumConverter<T> : JsonConverter where T : struct {
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
			var value = (string)reader.Value;
			return value == "" ? Enum.GetValues(_type).GetValue(0) : Enum.Parse(_type, value);
		}

		public override bool CanConvert(Type objectType) => _type == objectType;
	}
}