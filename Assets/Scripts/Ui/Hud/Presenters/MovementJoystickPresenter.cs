using System;
using Common;
using UnityEngine;
using Utopia;

namespace Ui.Hud {
	[InstallerGenerator(InstallerId.Ui)]
	public class MovementJoystickPresenter : IDisposable{
		public Signal<Vector2> OnMovementChange = new();

		public void Dispose() => OnMovementChange?.Dispose();
	}
}
