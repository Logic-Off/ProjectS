using System.Collections.Generic;
using Zenject;
using Zentitas;

namespace Common {
	public abstract class AStrategy<TEntity, TCommand>
		where TEntity : Entity
		where TCommand : IStrategyCommand<TEntity> {
		[Inject] protected List<TCommand> _commands;

		public virtual void Execute(TEntity entity) {
			foreach (var command in _commands)
				if (command.Accept(entity))
					command.Apply(entity);
		}
	}

	public abstract class AStrategy<TEntity, TAdditionalValue, TCommand>
		where TEntity : Entity
		where TCommand : IStrategyCommand<TEntity, TAdditionalValue> {
		[Inject] protected List<TCommand> _commands;

		public virtual void Execute(TEntity entity, TAdditionalValue additionalValue) {
			foreach (var command in _commands)
				if (command.Accept(entity, additionalValue))
					command.Apply(entity, additionalValue);
		}
	}
}