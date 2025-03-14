using System;
using Common;
using UnityEngine;
using Utopia;

namespace Ui.Draggable {
	[InstallerGenerator(InstallerId.Ui)]
	public class DraggablePresenter : IDisposable {
		public readonly Signal OnHide = new();

		public readonly EventProperty<bool> IsVisible = new(false);
		public readonly EventProperty<string> IconId = new(string.Empty);
		public readonly EventProperty<Vector2> Position = new(Vector2.zero);

		public void Dispose() {
			OnHide?.Dispose();
			IsVisible?.Dispose();
			IconId?.Dispose();
			Position?.Dispose();
		}
	}
}