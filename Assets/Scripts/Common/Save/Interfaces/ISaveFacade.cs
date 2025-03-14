namespace Ecs.Save {
	public interface ISaveFacade {
		void AddListener(ISaveListener listener);
		void RemoveListener(ISaveListener listener);
		void Save();
	}
}