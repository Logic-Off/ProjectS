using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

namespace Ecs.Common {
	public abstract class AEcsBehaviour<T, V> : MonoBehaviour where T : class, IEntity where V : AEcsChildBehaviour<T> {
		protected T _entity;
		[SerializeField] private List<V> _children;

		public virtual void Link(T entity) {
			_entity = entity;
			gameObject.Link(_entity);
			_entity.OnDestroyEntity += OnDestroyEntity;

			foreach (var child in _children)
				child.Link(entity);
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