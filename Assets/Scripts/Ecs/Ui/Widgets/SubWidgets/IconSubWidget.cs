using Ui;
using UnityEngine;
using UnityEngine.UI;

namespace Ecs.Ui {
	public sealed class IconSubWidget : ASubWidget, IOnSpriteChangeEventListener {
		[SerializeField] private Image _image;
		
		public override void Build(UiContext context, UiEntity parent, UiEntity element) => element.SubscribeOnSpriteChange(this);

		public void OnChangeSprite(UiEntity entity) => _image.sprite = entity.Sprite.Value;
	}
}