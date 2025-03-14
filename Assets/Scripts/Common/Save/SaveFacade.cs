using System.Collections.Generic;
using Utopia;

namespace Ecs.Save {
	[InstallerGenerator(InstallerId.Project, 3)]
	public class SaveFacade : ISaveFacade {
		private readonly List<ISaveListener> _listeners = new();
		private readonly IDataService _dataService;

		public SaveFacade(IDataService dataService) => _dataService = dataService;

		public void AddListener(ISaveListener listener) {
			_listeners.Add(listener);
			_listeners.Sort((a, b) => a.Order.CompareTo(b.Order));
		}

		public void RemoveListener(ISaveListener listener) => _listeners.Remove(listener);

		public void Save() {
			foreach (var listener in _listeners)
				listener.Save();

			_dataService.Save();
		}
	}
}