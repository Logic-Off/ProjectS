using TMPro;
using Ui;
using UnityEngine;

namespace Ecs.Ui {
	public sealed class LabelSubWidget : ASubWidget, IOnStringChangeEventListener {
		[SerializeField] private TextMeshProUGUI _text;

		public override void Build(UiContext context, UiEntity parent, UiEntity element) => element.SubscribeOnStringChange(this);

		public void OnChangeString(UiEntity entity) => _text.SetText(entity.String.Value);
	}
}