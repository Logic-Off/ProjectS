using System;
using System.Collections.Generic;
using Common;
using Cysharp.Threading.Tasks;
using Ecs.Ui;
using Utopia;

namespace Ui.Cheats {
	[InstallerGenerator(InstallerId.Cheats)]
	public class CheatsInteractor {
		private int _index = 0;

		private readonly Dictionary<string, ICheat> _cheats = new();
		private readonly ISmallCheatButtonPool _smallCheatButtonPool;
		private readonly ICheatButtonPool _cheatButtonPool;
		private readonly ICheatHeaderPool _cheatHeaderPool;
		private readonly ICheatTextPool _cheatTextPool;
		private readonly CheatsPresenter _presenter;
		private readonly IWindowRouter _windowRouter;

		public CheatsInteractor(
			List<ICheat> cheats,
			ISmallCheatButtonPool smallCheatButtonPool,
			ICheatButtonPool cheatButtonPool,
			CheatsPresenter presenter,
			IWindowRouter windowRouter,
			ICheatHeaderPool cheatHeaderPool,
			ICheatTextPool cheatTextPool
		) {
			_smallCheatButtonPool = smallCheatButtonPool;
			_cheatButtonPool = cheatButtonPool;
			_presenter = presenter;
			_windowRouter = windowRouter;
			_cheatHeaderPool = cheatHeaderPool;
			_cheatTextPool = cheatTextPool;

			cheats.Sort((a, b) => a.Order.CompareTo(b.Order));
			foreach (var cheat in cheats)
				_cheats.Add(cheat.Name, cheat);
		}

		public void OnActivate() => Activate();

		private async UniTaskVoid Activate() {
			foreach (var (name, cheat) in _cheats) {
				cheat.Create();
				var button = await _smallCheatButtonPool.Get(_presenter.PanelId.Value, _presenter.ButtonsContainer.Value, cheat);
				button.SubscribeOnClickedChange(OnCreateSubButtons);
				_presenter.Buttons.Value.Add(name, button);
			}
		}

		private void OnCreateSubButtons(UiEntity entity) {
			_presenter.CurrentCheat.Value = entity.String.Value;
			Clear();
			OnRedraw(entity);
		}

		private async UniTaskVoid OnRedraw(UiEntity entity) {
			var cheat = _cheats[entity.String.Value];
			foreach (var (name, entry) in cheat.Objects) {
				switch (entry.Type) {
					case ECheatElementType.Header:
						var header = await _cheatHeaderPool.Get(_presenter.PanelId.Value, _presenter.ContentContainer.Value, $"{entry.Type}_{_index++}");
						header.ReplaceString(entry.Text);
						_presenter.Headers.Value.Add(header);
						break;
					case ECheatElementType.Button:
						var button = await _cheatButtonPool.Get(_presenter.PanelId.Value, _presenter.ContentContainer.Value, $"{entry.Type}_{_index++}");
						button.SubscribeOnClickedChange(x => cheat.ButtonCallbacks[entry.Name].Invoke());

						button.ReplaceString(entry.Text);
						_presenter.SubButtons.Value.Add(button);
						break;
					case ECheatElementType.Text:
						var text = await _cheatTextPool.Get(_presenter.PanelId.Value, _presenter.ContentContainer.Value, $"{entry.Type}_{_index++}");
						text.ReplaceString(entry.Text);
						_presenter.Texts.Value.Add(text);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		private void Clear() {
			foreach (var entity in _presenter.Headers.Value)
				_cheatHeaderPool.Return(entity);
			_presenter.Headers.Value.Clear();

			foreach (var entity in _presenter.Texts.Value)
				_cheatTextPool.Return(entity);
			_presenter.Texts.Value.Clear();

			foreach (var entity in _presenter.SubButtons.Value)
				_cheatButtonPool.Return(entity);
			_presenter.SubButtons.Value.Clear();
		}

		public void OnRefresh() {
			if (_presenter.CurrentCheat.Value.IsNullOrEmpty())
				return;
			OnCreateSubButtons(_presenter.Buttons.Value[_presenter.CurrentCheat.Value]);
		}

		public void OnClose() => _windowRouter.OnBack();
	}
}