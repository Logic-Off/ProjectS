using Cysharp.Threading.Tasks;

namespace Ui {
	public interface IPanelBuilder {
		UniTask<UiEntity> LoadPanel(UiContext context, UiEntity parent);
	}
}