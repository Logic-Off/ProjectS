using Ui;

namespace Ecs.Ui {
	public sealed class BaseWidget : AWidget {
		protected override EUiType Type => EUiType.Element;
	}
}