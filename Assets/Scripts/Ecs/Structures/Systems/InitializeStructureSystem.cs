using Ecs.Common;
using UnityEngine;
using Utopia;

namespace Ecs.Structures {
	[InstallerGenerator(InstallerId.Game, 400)]
	public class InitializeStructuresSystem : IOnSceneLoadedListener {
		private readonly StructureContext _structure;

		public InitializeStructuresSystem(StructureContext structure) => _structure = structure;

		public void OnSceneLoaded() {
			var structures = Object.FindObjectsByType<StructureBehaviour>(FindObjectsSortMode.None);
			foreach (var structure in structures) {
				var transform = structure.transform;
				var entity = _structure.CreateEntity();
				structure.Link(entity);

				entity.AddId(IdGenerator.GetNext());
				var name = GetFullName(transform);
				entity.AddName(name);
				entity.AddPosition(transform.position);
				entity.AddRotation(transform.rotation);
				entity.AddInstanceId(entity.HasCollider ? entity.Collider.Value.GetInstanceID() : structure.gameObject.GetInstanceID());
			}
		}

		private string GetFullName(Transform transform) {
			var name = string.Empty;
			if (transform.parent != null)
				name = GetFullName(transform.parent) + ".";

			name += transform.name;
			return name;
		}
	}
}