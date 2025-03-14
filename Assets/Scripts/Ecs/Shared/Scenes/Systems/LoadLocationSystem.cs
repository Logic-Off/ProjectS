using System;
using System.Collections.Generic;
using Zentitas;
using Utopia;

namespace Ecs.Shared {
	[InstallerGenerator(InstallerId.Project)]
	public class LoadLocationSystem : ReactiveSystem<SharedEntity> {
		private readonly SharedContext _shared;
		private readonly ILoadSceneController _controller;

		public LoadLocationSystem(SharedGlobalContext shared, ILoadSceneController controller) : base(shared.Context) {
			_shared = shared;
			_controller = controller;
		}

		protected override ICollector<SharedEntity> GetTrigger(IContext<SharedEntity> context)
			=> context.CreateCollector(SharedMatcher.LoadLocation.Added());

		protected override bool Filter(SharedEntity entity)
			=> entity.IsScene && entity.HasLoadLocation;

		protected override void Execute(List<SharedEntity> entities) {
			OnLoad();
		}

		private async void OnLoad() {
			var entity = _shared.SceneEntity;
			if (entity.IsLoading)
				return;

			var location = entity.LoadLocation.Value;
			await _controller.OnLoad(location);
			entity.ReplaceCurrentLocation(location);
		}
	}
}