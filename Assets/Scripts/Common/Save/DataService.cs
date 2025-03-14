using System.Collections.Generic;
using Utopia;

namespace Ecs.Save {
	[InstallerGenerator(InstallerId.Project, 1)]
	public class DataService : IDataService {
		private Dictionary<string, object> _database = new();

		public DataService() {
			foreach (var key in ES3.GetKeys()) {
#if DEBUG
				D.Warning($"Load save: ", key);
#endif
				_database.Add(key, ES3.Load(key));
			}
		}

		public void SetObject(string key, object value) => _database[key] = value;

		public T GetObject<T>(string key, T defaultValue = default) {
			if (_database.ContainsKey(key))
				return (T)_database[key];
			return defaultValue;
		}

		public void Save() {
			foreach (var pair in _database)
				ES3.Save(pair.Key, pair.Value);
		}
	}
}