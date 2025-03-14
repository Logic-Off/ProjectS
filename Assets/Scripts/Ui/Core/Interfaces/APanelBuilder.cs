using Cysharp.Threading.Tasks;
using Ecs.Common;
using Ecs.Ui;
using Zenject;

namespace Ui {
	public abstract class APanelBuilder : IPanelBuilder {
		protected abstract EPanelName PanelName { get; }
		private bool _loading;
		private bool _isVisible;
		private string _name;

		[Inject] private AsyncPanelFactory _reference;
		[Inject] private UiContext _ui;

		public virtual async UniTask<UiEntity> LoadPanel(UiContext context, UiEntity parent) {
			while (_loading)
				await UniTask.Yield();

			var entity = context.GetEntityWithPanelName(PanelName);
			if (entity != null)
				return entity;

			_loading = true;
			var view = await _reference.GetView();
			entity = CreatePanelEntity(context, parent, view as PanelView);
			_name = entity.Name.Value;
			Build(context, entity);
			_loading = false;

			return entity;
		}

		private UiEntity CreatePanelEntity(UiContext context, UiEntity parent, PanelView panel) {
			var entity = context.CreateEntity();
			entity.AddId(IdGenerator.GetNext());
			entity.AddName($"{PanelName}");
			entity.AddParent(parent.Id.Value);
			entity.AddPanelName(PanelName);
			entity.IsActive = parent.IsActive;
			entity.IsVisible = true;

			panel.Build(context, entity);

			entity.SubscribeOnActiveChange(OnChangeActive);
			entity.SubscribeOnVisibleChange(OnChangeVisible);

			OnChangeVisible(entity);

			return entity;
		}

		protected virtual void Build(UiContext context, UiEntity panelEntity) {
			BindView(context, panelEntity);
			BindInteractor();
			Activate();
		}

		protected virtual void BindView(UiContext context, UiEntity entity) { }
		protected virtual void BindInteractor() { }
		protected virtual void Activate() { }
		protected virtual void OnShow() { }
		protected virtual void OnHide() { }

		protected UiEntity Find(string targetName) => _ui.GetEntityWithName($"{_name}.{targetName}");

		public void OnChangeVisible(UiEntity entity) => OnVisible(entity.IsActive && entity.IsVisible);

		public void OnChangeActive(UiEntity entity) => OnVisible(entity.IsActive && entity.IsVisible);

		private void OnVisible(bool isVisible) {
			if (_isVisible == isVisible)
				return;
			_isVisible = isVisible;
			if (isVisible)
				OnShow();
			else
				OnHide();
		}
	}
}