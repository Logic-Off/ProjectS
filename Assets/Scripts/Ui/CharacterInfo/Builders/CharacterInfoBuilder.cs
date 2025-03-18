using Utopia;

namespace Ui.CharacterInfo {
	[InstallerPanelGenerator(InstallerId.Ui)]
	public class CharacterInfoBuilder : APanelBuilder {
		protected override EPanelName PanelName => EPanelName.CharacterInfo;
		private readonly CharacterInfoPresenter _presenter;
		private readonly CharacterInfoInteractor _interactor;

		public CharacterInfoBuilder(CharacterInfoPresenter presenter, CharacterInfoInteractor interactor) {
			_presenter = presenter;
			_interactor = interactor;
		}

		protected override void BindView(UiContext context, UiEntity entity) {
			_presenter.IsVisible.AddListener(x => entity.IsVisible = x);
		}

		protected override void BindInteractor() {
 			base.BindInteractor();
		}

		protected override void Activate() {
			base.Activate();
		}
	}
}
