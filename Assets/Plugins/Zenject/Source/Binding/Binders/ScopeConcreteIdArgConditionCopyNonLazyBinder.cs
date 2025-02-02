namespace Zenject
{
    [NoReflectionBaking]
    public class ScopeConcreteIdArgConditionCopyNonLazyBinder : ConcreteIdArgConditionCopyNonLazyBinder
    {
        public ScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo bindInfo)
            : base(bindInfo)
        {
        }

        public ConcreteIdArgConditionCopyNonLazyBinder AsCached()
        {
            BindInfo.Scope = EScopeTypes.Singleton;
            return this;
        }

        public ConcreteIdArgConditionCopyNonLazyBinder AsSingle()
        {
            BindInfo.Scope = EScopeTypes.Singleton;
            BindInfo.MarkAsUniqueSingleton = true;
            return this;
        }

        // Note that this is the default so it's not necessary to call this
        public ConcreteIdArgConditionCopyNonLazyBinder AsTransient()
        {
            BindInfo.Scope = EScopeTypes.Transient;
            return this;
        }
    }
}
