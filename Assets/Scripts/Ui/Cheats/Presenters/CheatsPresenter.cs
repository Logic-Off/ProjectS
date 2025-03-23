using System;
using System.Collections.Generic;
using Common;
using Ecs.Common;
using UnityEngine;
using Utopia;

namespace Ui.Cheats {
	[InstallerGenerator(InstallerId.Cheats)]
	public class CheatsPresenter : IDisposable {
		public readonly Signal OnActivate = new();
		public readonly Signal OnRefresh = new();
		public readonly Signal OnClose = new();
		public readonly EventProperty<Id> PanelId = new();
		public readonly EventProperty<RectTransform> ContentContainer = new();
		public readonly EventProperty<RectTransform> ButtonsContainer = new();
		public readonly EventProperty<List<UiEntity>> SubButtons = new(new List<UiEntity>());
		public readonly EventProperty<List<UiEntity>> Headers = new(new List<UiEntity>());
		public readonly EventProperty<List<UiEntity>> Texts = new(new List<UiEntity>());
		public readonly EventProperty<Dictionary<string, UiEntity>> Buttons = new(new Dictionary<string, UiEntity>());
		public readonly EventProperty<string> CurrentCheat = new(string.Empty);

		public void Dispose() {
			OnActivate?.Dispose();
			OnRefresh?.Dispose();
			OnClose?.Dispose();
			PanelId?.Dispose();
			ContentContainer?.Dispose();
			ButtonsContainer?.Dispose();
			SubButtons?.Dispose();
			Headers?.Dispose();
			Texts?.Dispose();
			Buttons?.Dispose();
			CurrentCheat?.Dispose();
		}
	}
}