using System.Collections.Generic;
using Utopia;

namespace Ecs.AI {
	[InstallerGenerator(InstallerId.Game)]
	public class UtilityAI {
		private readonly Dictionary<EAiAction, IAction> _actions = new();

		public UtilityAI(List<IAction> actions) {
			foreach (var action in actions)
				_actions.Add(action.Name, action);
		}

		public void ChooseBestAction(GameEntity entity) {
			IAction bestAction = null;
			var highestScore = -1f;

			foreach (var action in _actions.Values) {
				var score = action.GetScore(entity);
				if (score <= highestScore)
					continue;
				highestScore = score;
				bestAction = action;
			}

			if (bestAction == null)
				return;

			if (entity.PreviousAiAction.Value != bestAction.Name) {
				_actions[entity.PreviousAiAction.Value].Exit(entity);
				bestAction.Enter(entity);
				entity.ReplacePreviousAiAction(bestAction.Name);
			}

			bestAction.Execute(entity);
		}
	}
}