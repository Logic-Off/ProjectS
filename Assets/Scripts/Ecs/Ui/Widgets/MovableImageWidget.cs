namespace Ecs.Ui {
	public sealed class MovableImageWidget : ImageWidget {
		public override UiEntity Build(UiContext context, UiEntity parent) {
			var element = base.Build(context, parent);
			element.AddVector2(_rect.position);
			element.SubscribeOnVector2Change(OnChangeVector2);
			return element;
		}

		public override void OnChangeSprite(UiEntity entity) {
			_icon.gameObject.SetActive(entity.Sprite.Value != null);
			base.OnChangeSprite(entity);
		}

		public void OnChangeVector2(UiEntity entity) => _rect.position = entity.Vector2.Value;
	}
}