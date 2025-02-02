using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Common;
using LogicOff.Extensions;

namespace LogicOff.DatabaseDownloader {
	/// <summary>
	///   Author: Andrey Abramkin
	/// </summary>
	public static class TableConverter {
		public static string ConvertDataTableToJson(Dictionary<int[], TableEntry> schema, List<List<string>> tableRows) {
			var builder = new StringBuilder();
			if (tableRows.Count == 0)
				return builder.ToString();

			builder.Append("[").Append('\n');

			for (var i = 0; i < tableRows.Count; i++) {
				if (tableRows[i][0].IsNullOrEmpty())
					continue;
				if (i > 0)
					builder.Append(",\n");
				builder.Append("{").Append('\n');

				var currentIndex = 0;
				foreach (var header in schema) {
					CollectItems(tableRows, header, builder, i);

					if (schema.Count > currentIndex + 1)
						builder.Append(',');

					builder.Append('\n');
					currentIndex++;
				}

				builder.Append("}");
			}

			builder.Append("\n]");

			return builder.ToString();
		}

		private static void CollectItems(
			List<List<string>> tableRows,
			KeyValuePair<int[], TableEntry> header,
			StringBuilder builder,
			int i
		) {
			var columns = tableRows[i];
			var entry = header.Value;
			if (entry.IsArray) {
				builder.Append('\"').Append(entry.Name).Append("\":[").Append('\n'); // Имя массива

				// Не стал флаг делать, по сути если есть элементы то это уже вертикальный массив
				if (entry.Elements != null) // Вертикальный массив
					OnVerticalArray(tableRows, builder, header, 0, i);
				else if (header.Key.Length > 1) // Горизонтальный массив
					OnCollectHorizontalArray(tableRows[i], builder, header);

				builder.Append("]"); // Закрываем массив
			} else {
				if (entry.IsObject)
					OnObject(tableRows, header, i, builder, entry);
				else
					OnDefaultValue(builder, entry.Name, columns[header.Key[0]]);
			}
		}
		
		private static void OnObject(
			List<List<string>> tableRows,
			KeyValuePair<int[], TableEntry> header,
			int i,
			StringBuilder builder,
			TableEntry entry
		) {
			var onCollectValues = TryCollectValues(tableRows, header, i, out var tableElements);
			if (!onCollectValues)
				return;
			builder.Append('\"').Append(entry.Name).Append("\":").Append('\n'); // Имя Объекта
			builder.Append("{\n");
			builder.Append(tableElements);
			builder.Append("\n}");
		}

		private static void OnVerticalArray(
			List<List<string>> tableRows,
			StringBuilder builder,
			KeyValuePair<int[], TableEntry> header,
			int parentColumn,
			int currentIndex
		) {
			var isObject = header.Value.IsObject;
			for (var index = currentIndex; index < tableRows.Count; index++) {
				// Проверяем парента, если там не пустая ячейка, то значит начался следующий элемент
				if (index != currentIndex && !tableRows[index][parentColumn].IsNullOrEmpty())
					break;

				// Если ячейка пустая - пропускаем
				if (tableRows[index][header.Key[0]].IsNullOrEmpty())
					continue;

				var isCollectValues = TryCollectValues(tableRows, header, index, out var tableElements);
				if (isCollectValues) {
					if (isObject)
						builder.Append("{\n");
					builder.Append(tableElements);
					if (isObject)
						builder.Append("\n}");
				}

				// Выходим за пределы или начался следующий объект
				if (tableRows.Count > index + 1 && !tableRows[index + 1][0].IsNullOrEmpty() || tableRows.Count == index + 1) {
					builder.Append('\n');
					break;
				}

				// Добавили элемент и это не конец
				if (isCollectValues)
					builder.Append(",\n");
			}
		}

		private static bool TryCollectValues(
			List<List<string>> tableRows,
			KeyValuePair<int[], TableEntry> header,
			int currentIndex,
			out StringBuilder builder
		) {
			builder = new StringBuilder();
			var result = false;
			var i = 0;
			var elements = header.Value.Elements;
			foreach (var pair in elements) {
				i++;
				// Если ячейка пустая, то пропускаем, ибо нафига она в массиве? Возвожно в каких-то случаях нужна, если это локализация допустим, но там горизональный массив скорее будет
				var value = tableRows[currentIndex][pair.Key[0]];
				if (value.IsNullOrEmpty())
					continue;
				result = true;

				if (pair.Value.IsArray) {
					builder.Append('\"').Append(pair.Value.Name).Append("\":[").Append('\n'); // Имя массива

					// Вертикальный массив
					if (pair.Value.Elements != null)
						OnVerticalArray(tableRows, builder, pair, header.Key[0], currentIndex);
					else if (header.Key.Length > 1) // Горизонтальный массив
						OnCollectHorizontalArray(tableRows[i], builder, header);

					builder.Append("]"); // Закрываем массив
				} else {
					// Не уверен что стоит таким способом записывать объект, подходит для простых типов float, string, но не объектов
					if (elements.Count == 1 && pair.Value.Name.IsNullOrEmpty())
						builder.Append(ConvertValue(value));
					else
						OnDefaultValue(builder, pair.Value.Name, value);
				}

				if (elements.Count > i)
					builder.Append(",\n");
			}

			return result;
		}

		private static void OnCollectHorizontalArray(
			List<string> columns,
			StringBuilder builder,
			KeyValuePair<int[], TableEntry> header
		) {
			foreach (var i in header.Key) {
				var cell = columns[i].Trim();
				builder.Append(ConvertValue(cell));
				builder.Append(columns.Count > i + 1 ? ",\n" : "\n");
			}
		}

		private static StringBuilder OnDefaultValue(StringBuilder builder, string name, string value) {
			value = ConvertValue(value);
			return builder.Append('\"')
				.Append(name)
				.Append("\":")
				.Append(value);
		}

		private static string ConvertValue(string value) {
			var isNumeric = double.TryParse(value, out var number);
			// InvariantCulture заменяет запятую на точку
			if (!isNumeric)
				value = value.Replace("\"", "\\" + "\"");
			return isNumeric ? number.ToString(CultureInfo.InvariantCulture) : $"\"{value}\"";
		}
	}
}