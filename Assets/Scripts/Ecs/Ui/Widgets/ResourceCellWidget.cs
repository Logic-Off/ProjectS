using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ecs.Ui {
	public class ResourceCellWidget : DragAndDropWidget, IOnSpriteChangeEventListener, IOnIntChangeEventListener, IOnColorChangeEventListener {
		protected override EUiType Type => EUiType.Cell;

		[SerializeField] protected Image _icon;
		[SerializeField] protected TextMeshProUGUI _count;

		public override UiEntity Build(UiContext context, UiEntity parent) {
			var element = base.Build(context, parent);

			element.SubscribeOnSpriteChange(this);
			element.SubscribeOnIntChange(this);
			element.AddVector2(_icon.rectTransform.anchoredPosition);
			element.SubscribeOnVector2Change(OnChangeVector2);
			element.SubscribeOnColorChange(this);

			return element;
		}

		public void OnChangeSprite(UiEntity entity) {
			_icon.gameObject.SetActive(entity.Sprite.Value != null);
			_icon.sprite = entity.Sprite.Value;
		}

		public void OnChangeInt(UiEntity entity) {
			_count.gameObject.SetActive(entity.Int.Value > 1);
			_count.SetText($"{entity.Int.Value}");
		}

		public void OnChangeVector2(UiEntity entity) => _icon.rectTransform.anchoredPosition = entity.Vector2.Value;
		public void OnChangeColor(UiEntity entity) => _icon.color = entity.Color.Value;
	}
}