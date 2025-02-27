using Zentitas;

public abstract class AGlobalContext<T> where T : IContext {
	public T Context { get; }
	protected AGlobalContext(T context) => Context = context;
	public static implicit operator T(AGlobalContext<T> provider) => provider.Context;
}