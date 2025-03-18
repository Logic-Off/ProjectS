using Zentitas;

namespace Common {
	public interface IStrategyCommand<TEntity> where TEntity : Entity {
		bool Accept(TEntity command);
		void Apply(TEntity animationEvent);
	}

	public interface IStrategyCommand<TEntity, TAdditionalValue> where TEntity : Entity {
		bool Accept(TEntity entity, TAdditionalValue value);
		void Apply(TEntity entity, TAdditionalValue value);
	}
}