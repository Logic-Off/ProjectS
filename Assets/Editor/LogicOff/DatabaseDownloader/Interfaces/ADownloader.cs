using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LogicOff.DatabaseDownloader.Google;
using Newtonsoft.Json;
using UnityEngine;

namespace LogicOff.DatabaseDownloader {
	public abstract class ADownloader<T> : IDownloader {
		private Action<T[]> _callback;
		private string _key;
		protected virtual string Key => _key;
		public SheetEntry Entry;

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

		public ADownloader() { }

		public ADownloader(string key, SheetEntry entry, Action<T[]> callback) {
			_key = key;
			Entry = entry;
			_callback = callback;
		}

		public void Set(string key, SheetEntry entry, List<T> list) {
			_key = key;
			Entry = entry;
			_callback = x => {
				list.Clear();
				list.AddRange(x);
			};
		}

		public void Set(string key, SheetEntry entry, Action<T[]> callback) {
			_key = key;
			Entry = entry;
			_callback = callback;
		}

		public async Task Download() => await DownloadSheet();

		public async Task<T[]> DownloadSheet() {
			try {
				var request =
					await SpreadsheetManager.ReadPublicSpreadsheet(new Spreadsheet(Key, Entry.Name, Entry.StartCell, Entry.EndCell, Entry.TitleColumn, Entry.TitleRow));

				var json = TableConverter.ConvertDataTableToJson(Schema, request);
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
			D.Log("[ADownloader]\n", parse);
			return JsonConvert.DeserializeObject<JsonItems<T>>(parse, converters)?.List.ToArray();
		}

		[Serializable]
		public class JsonItems<T> {
			public List<T> List;
		}
	}
}