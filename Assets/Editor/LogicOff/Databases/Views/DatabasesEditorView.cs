using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace LogicOff.Databases {
	/// <summary>
	/// </summary>
	public sealed class DatabasesEditorView {
		private Object _database;
		private Editor _editor;

		public readonly VisualElement InfoContainer;
		public readonly ScrollView ScrollContainer;
		public readonly IMGUIContainer IMGUIContainer;
		public readonly ListView PrimeList;
		public readonly ListView SubList;

		public DatabasesEditorView(VisualElement parent) {
			// После переноса в другой проект необходимо править путь
			parent.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/LogicOff/Databases/styles.uss"));
			var toolbar = new UnityEditor.UIElements.Toolbar();
			toolbar.AddToClassList("databases-toolbar");

			var container = new VisualElement();
			container.AddToClassList("databases-mainContainer");

			var subContainer = new VisualElement();
			subContainer.AddToClassList("databases-subContainer");

			PrimeList = CreateListView("PrimeList");
			SubList = CreateListView("SubList");

			InfoContainer = new VisualElement();
			InfoContainer.AddToClassList("databases-infoContainer");
			ScrollContainer = new ScrollView();

			IMGUIContainer = new IMGUIContainer(OnGUI);
			ScrollContainer.Add(IMGUIContainer);

			container.Add(PrimeList);
			container.Add(SubList);

			InfoContainer.Add(ScrollContainer);
			subContainer.Add(InfoContainer);

			container.Add(subContainer);

			parent.Add(toolbar);
			parent.Add(container);
		}

		private ListView CreateListView(string name) {
			var listView = new ListView();
			listView.name = name;

			listView.AddToClassList("databases-listView");
			listView.makeItem = () => {
				var label = new Label();
				label.AddToClassList("databases-listView-label");
				return label;
			};
			listView.itemHeight = 20;
			listView.selectionType = SelectionType.Multiple;
			listView.style.height = new StyleLength(StyleKeyword.Auto);
			listView.style.alignSelf = Align.Stretch;
			listView.style.flexGrow = 1.0f;

			return listView;
		}

		public void SetDatabase(Object database) {
			_database = database;
			if (database == null && ScrollContainer.Contains(IMGUIContainer))
				ScrollContainer.Remove(IMGUIContainer);
			else if (database != null && !ScrollContainer.Contains(IMGUIContainer))
				ScrollContainer.Add(IMGUIContainer);
		}

		private void OnGUI() {
			if (_database != null)
				Editor.DrawFoldoutInspector(_database, ref _editor);
		}

		public void SetPrimeListElements(List<string> values) {
			Action<VisualElement, int> bindItem = (e, i) => ((Label)e).text = values[i];
			SetListElements(PrimeList, values, bindItem);
		}

		public void SetSubListElements(List<Object> values) {
			SetListElements(
				SubList,
				values,
				(element, i) => {
					if (values.Count <= i)
						return;
					((Label)element).text = ToReadableCamelCase(values[i].name);
					var f = Color.HSVToRGB(Max((values[i].name[0] - 'A'), 0) / 26f, 1f, 0.8f);
					element.style.borderLeftColor = new StyleColor(f);
					element.style.borderLeftWidth = new StyleFloat(5f);
					element.style.paddingLeft = new StyleLength(new Length(5f, LengthUnit.Pixel));
				}
			);
			SubList.style.display = values.Count > 1 ? DisplayStyle.Flex : DisplayStyle.None;
			// if (values.Count > 0)
			// 	SubList.selectedIndex = 0;
		}

		private int Max(int a, int b) => a > b ? a : b;

		private void SetListElements<T>(ListView listView, List<T> values, Action<VisualElement, int> bindItem) {
			listView.bindItem = bindItem;
			listView.itemsSource = values;
		}

		public string ToReadableCamelCase(string input, string whitespace = " ") {
			var r = new Regex(
				@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])",
				RegexOptions.IgnorePatternWhitespace
			);

			return r.Replace(input, whitespace);
		}
	}
}