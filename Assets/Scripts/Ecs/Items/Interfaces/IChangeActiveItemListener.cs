namespace Ecs.Items {
	public interface IChangeActiveItemListener {
		bool CanActivate(ItemEntity item);
		void Activate(ItemEntity item);
		void Deactivate(ItemEntity item);
	}
}