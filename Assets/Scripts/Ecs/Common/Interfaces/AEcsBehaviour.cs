using System.Collections.Generic;
using UnityEngine;
using Zentitas;

namespace Ecs.Common {
	public abstract class AEcsBehaviour<T, V> : MonoBehaviour where T : class, IEntity where V : AEcsSubBehaviour<T> {
		protected T _entity;
		[SerializeField] private List<V> _subBehaviours;

		public virtual void Link(T entity) {
			_entity = entity;
			gameObject.Link(_entity);
			_entity.OnDestroyEntity += OnDestroyEntity;

			foreach (var subBehaviour in _subBehaviours)
				subBehaviour.Link(entity);
		}

		private void OnDestroyEntity(IEntity entity) {
			if (_entity == null)
				return;

			var link = gameObject.GetEntityLink();
			if (link != null && link.Entity != null)
				link.Unlink();
			if (entity.IsEnabled)
				_entity.OnDestroyEntity -= OnDestroyEntity;
			_entity = null;
		}
	}
}