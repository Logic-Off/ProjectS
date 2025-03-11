using System.Collections.Generic;

namespace Zenject {
	public class ZenjectListPool<T> : StaticMemoryPool<List<T>> {
		static ZenjectListPool<T> _instance = new ZenjectListPool<T>();

		public ZenjectListPool() {
			OnDespawnedMethod = OnDespawned;
		}

		public static ZenjectListPool<T> Instance { get { return _instance; } }

		void OnDespawned(List<T> list) {
			list.Clear();
		}
	}
}