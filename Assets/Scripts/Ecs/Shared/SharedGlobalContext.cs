namespace Ecs.Shared {
	public class SharedGlobalContext : AGlobalContext<SharedContext> {
		public SharedGlobalContext(SharedContext context) : base(context) { }
	}
}