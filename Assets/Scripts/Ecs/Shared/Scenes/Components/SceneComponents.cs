using Common;
using Zentitas;
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

	[Shared, DontDrawComponent]
	public sealed class LocationDataComponent : IComponent {
		public SceneInstance Scene;
		public SceneInstance Manager;
		public override string ToString() => $"";
	}

	[Shared]
	public sealed class LoadingComponent : IComponent { }
}