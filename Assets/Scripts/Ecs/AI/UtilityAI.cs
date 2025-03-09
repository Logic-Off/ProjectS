using System.Collections.Generic;
using Utopia;

namespace Ecs.AI {
	[InstallerGenerator(InstallerId.Game)]
	public class UtilityAI {
		private readonly List<IAction> _actions;
		public UtilityAI(List<IAction> actions) => _actions = actions;

		public void ChooseBestAction(GameEntity entity) {
			IAction bestAction = null;
			var highestScore = -1f;

			D.Error("[UtilityAI]", _actions.Count);
			foreach (var action in _actions) {
				var score = action.GetScore(entity);
				if (score <= highestScore)
					continue;
				highestScore = score;
				bestAction = action;
			}

			bestAction?.Execute(entity);
		}
	}
}