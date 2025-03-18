using Common;
using UnityEngine;

namespace Ecs.Ui {
	public sealed class LocalizationWidget : AWidget, IOnLocalizationChangeEventListener {
		protected override EUiType Type => EUiType.Label;
		[SerializeField] private LocalizationBehaviour _text;

		public override UiEntity Build(UiContext context, UiEntity parent) {
			var element = base.Build(context, parent);
			element.AddOnLocalizationChangeEventListener(this);
			return element;
		}

		public void OnChangeLocalization(UiEntity entity) => _text.SetKey(entity.Localization.Value.Key, entity.Localization.Value.Arguments);
	}
}