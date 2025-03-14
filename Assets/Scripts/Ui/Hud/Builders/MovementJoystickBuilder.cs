using Ecs.Ui;
using Utopia;

namespace Ui.Hud {
	[InstallerPanelGenerator(InstallerId.Ui)]
	public class MovementJoystickBuilder : APanelBuilder {
		protected override EPanelName PanelName => EPanelName.MovementJoystick;
		private readonly MovementJoystickPresenter _presenter;
		private readonly MovementJoystickInteractor _interactor;

		public MovementJoystickBuilder(MovementJoystickPresenter presenter, MovementJoystickInteractor interactor) {
			_presenter = presenter;
			_interactor = interactor;
		}

		protected override void BindView(UiContext context, UiEntity entity) {
			Find("Joystick").SubscribeOnVector2Change(x => _presenter.OnMovementChange.Fire(x.Vector2.Value));
		}

		protected override void BindInteractor() {
			_presenter.OnMovementChange.AddListener(_interactor.OnMovement);
		}
	}
}
