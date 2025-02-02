using System.Collections.Generic;

namespace LogicOff.DatabaseDownloader {
	/// <summary>
	///   Author: Andrey Abramkin
	/// </summary>
	public sealed class TableEntry {
		/// <summary>
		/// Имя переменной
		/// </summary>
		public string Name;
		
		/// <summary>
		/// Данные элемента, int - номер столбца, используется только для массивов
		/// </summary>
		public Dictionary<int[], TableEntry> Elements;

		/// <summary>
		/// Массив, может быть несколько вариаций вертикальный, с 1 столбцом, и горизонтальный когда столбцов больше 1
		/// </summary>
		public bool IsArray;

		/// <summary>
		/// Объект это элемент данных который хранит в себе множество других данных
		/// Объект - Как пример уровень игрока который хранит атаку, здоровье, защиту, если один тип данных, то это не объект
		/// </summary>
		public bool IsObject;

		public TableEntry(
			string name,
			Dictionary<int[], TableEntry> elements = null,
			bool isArray = false,
			bool isObject = false
		) {
			Name = name;
			Elements = elements;
			IsArray = isArray;
			IsObject = isObject;
		}
	}
}