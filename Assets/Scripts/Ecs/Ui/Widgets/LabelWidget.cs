using TMPro;
using UnityEngine;

namespace Ecs.Ui {
	public sealed class LabelWidget : AWidget, IOnStringChangeEventListener {
		protected override EUiType Type => EUiType.Label;
		[SerializeField] private TextMeshProUGUI _text;

		public override UiEntity Build(UiContext context, UiEntity parent) {
			var element = base.Build(context, parent);
			element.AddOnStringChangeEventListener(this);
			return element;
		}

		public void OnChangeString(UiEntity entity) => _text.SetText(entity.String.Value);
	}
}