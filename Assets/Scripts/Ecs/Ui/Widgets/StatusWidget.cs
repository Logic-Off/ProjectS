using UnityEngine;
using UnityEngine.UI;

namespace Ecs.Ui {
	public sealed class StatusWidget : AWidget, IOnSpriteChangeEventListener, IOnFloatChangeEventListener {
		protected override EUiType Type => EUiType.Element;

		[SerializeField] private Image _icon;
		[SerializeField] private Image _filledLine;
		[SerializeField] private Gradient _color;
		

		public override UiEntity Build(UiContext context, UiEntity parent) {
			var element = base.Build(context, parent);
			element.SubscribeOnSpriteChange(this);
			element.SubscribeOnFloatChange(this);
			return element;
		}

		public void OnChangeSprite(UiEntity entity) => _icon.sprite = entity.Sprite.Value;

		public void OnChangeFloat(UiEntity entity) {
			_filledLine.fillAmount = entity.Float.Value;
			var color = _color.Evaluate(entity.Float.Value);
			_icon.color = color;
			_filledLine.color = color;
		}
	}
}