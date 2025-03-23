using Utopia;
using Zentitas;

namespace Ecs.AI {
	[InstallerGenerator(InstallerId.Game)]
	public class UtilityAISystem : IUpdateSystem {
		private readonly IGroup<GameEntity> _group;

		private readonly UtilityAI _utilityAI;

		public UtilityAISystem(GameContext game, UtilityAI utilityAI) {
			_utilityAI = utilityAI;
			_group = game.GetGroup(GameMatcher.AllOf(GameMatcher.Npc).NoneOf(GameMatcher.Dead));
		}

		public void Update() {
			var list = ListPool<GameEntity>.Get();
			_group.GetEntities(list);
			foreach (var entity in list)
				_utilityAI.ChooseBestAction(entity);

			list.ReturnToPool();
		}
	}
}