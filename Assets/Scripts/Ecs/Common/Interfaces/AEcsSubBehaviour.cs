using Zentitas;
using UnityEngine;

namespace Ecs.Common {
	public abstract class AEcsSubBehaviour<T> : MonoBehaviour where T : class, IEntity {
		public abstract void Link(T entity);
	}
}