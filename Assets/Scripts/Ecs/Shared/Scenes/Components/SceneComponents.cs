using Common;
using JCMG.EntitasRedux;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Ecs.Shared {
	[Shared, Unique]
	public sealed class SceneComponent : IComponent { }

	[Shared]
	public sealed class CurrentLocationComponent : IComponent {
		public LocationId Value;
	}

	[Shared]
	public sealed class LoadLocationComponent : IComponent {
		public LocationId Value;
	}

	[Shared]
	public sealed class LocationDataComponent : IComponent {
		public SceneInstance Scene;
		public SceneInstance Manager;
	}

	[Shared]
	public sealed class LoadingComponent : IComponent { }
}