using Ui;
using UnityEngine;
using UnityEngine.UI;

namespace Ecs.Ui {
	public class ImageWidget : AWidget, IOnSpriteChangeEventListener {
		protected override EUiType Type => EUiType.Icon;
		[SerializeField] protected Image _icon;

		public override UiEntity Build(UiContext context, UiEntity parent) {
			var element = base.Build(context, parent);
			element.SubscribeOnSpriteChange(this);
			return element;
		}

		public virtual void OnChangeSprite(UiEntity entity) => _icon.sprite = entity.Sprite.Value;

		protected override void OnValidate() {
			base.OnValidate();
			if (_icon is null)
				_icon = GetComponent<Image>();
		}
	}
}