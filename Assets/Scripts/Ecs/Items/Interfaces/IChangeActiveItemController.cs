namespace Ecs.Items {
	public interface IChangeActiveItemController {
		void Activate(ItemEntity item);
		void Deactivate(ItemEntity item);
	}
}