using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace LogicOff.DatabaseDownloader {
	/// <summary>
	///   Author: Andrey Abramkin
	/// </summary>
	public abstract class ADownloader<T> : IDownloader {
		private readonly Action<T[]> _callback;
		private readonly string _path = "https://docs.google.com/spreadsheets/d/{0}/export?exportFormat=tsv&gid={1}";
		private readonly string _key;
		private readonly string _id;
		protected virtual string Id => _id;

		protected virtual string Key => _key;

		protected virtual JsonConverter[] Converters => ConvertersList.Converters;

		/// <summary>
		///   Больше 1 элемента в ключе, означает что это массив с одним типом данных, массив будет собираться горизонтально по
		///   столбцам
		///   в Entry можно указать Elements, будет обозначать что это тоже массив, массив будет собираться вертикально и
		///   горизонтально в зависимости от количества элементов,
		///   если больше 1 элемента должны быть указаны имена переменных
		///   Так же есть JsonSchema но пока отказался от её использования т.к. файл собирается руками
		/// </summary>
		protected abstract Dictionary<int[], TableEntry> Schema { get; }

		public abstract string Name { get; }

		public ADownloader(string key, string id, Action<T[]> callback) {
			_key = key;
			_id = id;
			_callback = callback;
		}

		public async Task Download() => await DownloadSheet();

		public async Task<T[]> DownloadSheet() {
			try {
				var request = UnityWebRequest.Get(string.Format(_path, Key, Id));
				request.timeout = 10;
				request.SendWebRequest();

				while (!request.isDone)
					await Task.Delay(10);

				var lines = new List<string>(request.downloadHandler.text.Split('\n'));

				var database = new List<List<string>>();
				for (var i = 2; i < lines.Count; i++) {
					var line = lines[i];
					var cells = line.Split('\t');
					for (var j = 0; j < cells.Length; j++)
						cells[j] = cells[j].Trim();
					
					database.Add(new List<string>(cells));
				}

				var json = TableConverter.ConvertDataTableToJson(Schema, database);
				// код json с построковым индексом
				var sb = new StringBuilder();
				sb.Append(Name).Append('\n');
				var ses = json.Split('\n');
				for (var i = 0; i < ses.Length; i++) {
					var s = ses[i];
					sb.Append(i).Append(": ").Append(s).Append('\n');
				}
				
				D.Warning("[ADownloader]", sb);
				var result = Read<T>(json, Converters);
				_callback?.Invoke(result);
				return result;
			} catch (Exception e) {
				Debug.LogException(e);
				return Array.Empty<T>(); // Асинхронка ждет завершения, возвращаем пустой массив
			}
		}

		private T[] Read<T>(string json, JsonConverter[] converters) {
			var _builder = new StringBuilder();
			var parse = _builder.Append("{ \"List\":").Append(json).Append("\n}").ToString();
			_builder.Clear();
			return JsonConvert.DeserializeObject<JsonItems<T>>(parse, converters)?.List.ToArray();
		}

		[Serializable]
		public class JsonItems<T> {
			public List<T> List;
		}
	}
}