namespace Ecs.Save {
	public interface ISaveListener {
		int Order { get; }
		void Save();
	}
}