using Ui;
using UnityEngine;
using UnityEngine.UI;

namespace Ecs.Ui {
	public class ButtonWidget : AWidget {
		protected override EUiType Type => EUiType.Button;
		[SerializeField] private Button _button;

		public override UiEntity Build(UiContext context, UiEntity parent) {
			var element = base.Build(context, parent);
			_button.onClick.AddListener(() => element.IsClicked = true);
			return element;
		}
	}
}