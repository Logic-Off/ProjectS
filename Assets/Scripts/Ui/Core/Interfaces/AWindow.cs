using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Ecs.Common;
using Ecs.Ui;
using UnityEngine;
using Zenject;
using Zentitas;

namespace Ui {
	public abstract class AWindow : IWindow, IDisposable {
		public enum EState {
			New,
			LoadingPanels,
			Builded
		}

		public abstract EWindowName Name { get; }
		public bool InProcessLoaded => _state != EState.Builded;

		protected UiEntity _entity;
		private EState _state = EState.New;
		private bool _isVisible;

		private readonly List<IPanelBuilder> _builders = new(0);
		private readonly List<UiEntity> _buildersEntities = new(3);

		[Inject] private DiContainer _container;
		[Inject] protected readonly UiContext _ui;

		public void Initialize() {
			_entity = _ui.CreateEntity();
			_entity.AddName($"Window.{Name}");
			_entity.AddId(IdGenerator.GetNext());
			_entity.AddUiType(EUiType.Window);
			_entity.AddWindowName(Name);

			AddPanelBuilders();
		}

		public void Dispose() {
			_builders.Clear();
			_buildersEntities.Clear();
			_entity = null;
		}

		public abstract void AddPanelBuilders();

		protected void AddBuilder<T>() where T : IPanelBuilder {
			var builder = _container.Resolve<T>();
			_builders.Add(builder);
		}

		public virtual void OnOpen() { }

		public async UniTaskVoid OnShow() {
			_isVisible = true;

			await BuildPanels(_ui, _entity);

			// Скорее надо пресекать если панель не собрана, а не ждать, пока оставил ожидание
			while (_state != EState.Builded)
				await UniTask.Yield();

			if (!_isVisible) // Если за время выполнения билда у нас вызвался OnHide, то не надо вызывать OnShow
				return;

			foreach (var entity in _buildersEntities) {
				entity.IsActive = true;
				entity.ReplaceSibling(ESibling.Last);
			}
		}

		protected async UniTask BuildPanels(UiContext context, UiEntity parent) {
			if (_state != EState.New)
				return;

			_state = EState.LoadingPanels;
			var list = ListPool<UniTask<UiEntity>>.Get();
			foreach (var builder in _builders)
				list.Add(builder.LoadPanel(context, parent));

			var result = await UniTask.WhenAll(list);
			_buildersEntities.AddRange(result);

			_state = EState.Builded;
			list.ReturnToPool();
		}

		public void OnHide() {
			_isVisible = false;
			if (_state != EState.Builded)
				return;

			foreach (var builder in _buildersEntities)
				builder.IsActive = false;
		}
	}
}