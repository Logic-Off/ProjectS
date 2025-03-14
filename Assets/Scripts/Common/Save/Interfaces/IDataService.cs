namespace Ecs.Save {
	public interface IDataService {
		void SetObject(string key, object value);
		T GetObject<T>(string key, T defaultValue = default);
		void Save();
	}
}