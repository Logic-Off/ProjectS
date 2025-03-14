using Utopia;

namespace Ui.Draggable {
	[InstallerGenerator(InstallerId.Ui)]
	public class DraggableInteractor {
		private readonly DraggablePresenter _presenter;
		
		public DraggableInteractor(DraggablePresenter presenter) {
			_presenter = presenter;
		}

		public void OnHide() {
			_presenter.IsVisible.Value = false;
		}
	}
}