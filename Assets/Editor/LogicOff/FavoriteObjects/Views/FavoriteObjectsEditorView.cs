using System;
using LogicOff.FavoriteObjects.Entries;
using UnityEngine.UIElements;

namespace LogicOff.FavoriteObjects.Views {
	/// <summary>
	///   Author: Andrey Abramkin
	/// </summary>
	public sealed class FavoriteObjectsEditorView {
		public readonly ListView MainList;
		public readonly Button OpenDatabaseButton;

		public FavoriteObjectsEditorView(VisualElement parent) {
			var container = new VisualElement();
			container.style.flexGrow = 1f;

			MainList = CreateListView("FavoriteItems");

			OpenDatabaseButton = new Button();
			OpenDatabaseButton.text = "Set database";

			container.Add(OpenDatabaseButton);
			container.Add(MainList);

			parent.Add(container);
		}

		private ListView CreateListView(string name) {
			var listView = new ListView();
			listView.name = name;

			listView.makeItem = () => {
				var container = new VisualElement();
				container.style.flexDirection = FlexDirection.Row;

				var image = new VisualElement();
				image.name = "Image";
				image.style.height = 20;
				image.style.width = 20;
				image.style.borderBottomLeftRadius = 5;
				image.style.borderBottomRightRadius = 5;
				image.style.borderTopLeftRadius = 5;
				image.style.borderTopRightRadius = 5;

				var label = new Label();
				label.style.height = 20;
				label.style.paddingTop = new StyleLength(new Length(3, LengthUnit.Pixel));

				container.Add(image);
				container.Add(label);
				return container;
			};
			listView.itemHeight = 20;
			listView.selectionType = SelectionType.Multiple;
			listView.style.height = new StyleLength(StyleKeyword.Auto);
			listView.style.alignSelf = Align.Stretch;
			listView.style.flexGrow = 1.0f;

			return listView;
		}

		public void SetFavoriteItems(FavoriteObjectItemEntry[] values) {
			Action<VisualElement, int> bindItem = (e, i) => {
				if (values.Length <= i)
					return;
				var value = values[i];
				var image = e.Q<VisualElement>("Image");
				image.style.backgroundImage = new StyleBackground(value.Icon);
				image.style.backgroundColor = value.IconBackgroundColor;

				var label = e.Q<Label>();
				label.text = value.Name;
			};
			SetListElements(MainList, values, bindItem);
		}

		private void SetListElements<T>(ListView listView, T[] values, Action<VisualElement, int> bindItem) {
			listView.bindItem = bindItem;
			listView.itemsSource = values;
		}
	}
}