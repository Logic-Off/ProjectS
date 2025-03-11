using Ecs.Common;
using UnityEngine;

namespace Ecs.Structures {
	public sealed class StructureBehaviour : AEcsBehaviour<StructureEntity, AStructureSubBehaviour> {
		[SerializeField] private EStructureType _type;

		public override void Link(StructureEntity entity) {
			entity.AddStructureType(_type);
			entity.AddTransform(transform);
			base.Link(entity);
		}
	}
}